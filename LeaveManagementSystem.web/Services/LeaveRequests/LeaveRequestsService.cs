using AutoMapper;
using LeaveManagementSystem.web.Models.LeaveAllocations;
using LeaveManagementSystem.web.Models.LeaveRequests;
using LeaveManagementSystem.web.Models.LeaveTypes;
using LeaveManagementSystem.web.Services.LeaveAllocations;
using LeaveManagementSystem.web.Services.Periods;
using LeaveManagementSystem.web.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.web.Services.LeaveRequests
{
    public partial class LeaveRequestsService(ApplicationDbContext _context, IMapper _mapper, IUserService _userService, IPeriodsService _periodsService, ILeavesAllocationService _leavesAllocationService) : ILeaveRequestsService
    {
        public async Task CreateLeaveRequest(LeaveRequestVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);
            var user = await _userService.GetLoggedInUser();
            leaveRequest.EmployeeId = user.Id;
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusCode.Pending;
            _context.Add(leaveRequest);
            
            await ChangeAllocationDays(leaveRequest, decrease: true);

            await _context.SaveChangesAsync();
        }

        public async Task<LeaveRequestReadOnlyVM> GetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(request => request.Employee)
                .Include(request => request.LeaveType)
                .ToListAsync();
            var approvedLeaveRequests = leaveRequests.Where(request => request.LeaveRequestStatusId == (int)LeaveRequestStatusCode.Approved).ToList();
            var pendingLeaveRequests = leaveRequests.Where(request => request.LeaveRequestStatusId == (int)LeaveRequestStatusCode.Pending).ToList();
            var rejectedLeaveRequests = leaveRequests.Where(request => request.LeaveRequestStatusId == (int)LeaveRequestStatusCode.Rejected).ToList();

            var leaveRequestReadOnlyVMs = leaveRequests.Select(request => new EmployeeLeaveRequestListVM
            {
                Id = request.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                LeaveType = request.LeaveType?.Name ?? string.Empty,
                LeaveRequestStatus = (LeaveRequestStatusCode)request.LeaveRequestStatusId,
                NumberOfDays = request.EndDate.DayNumber - request.StartDate.DayNumber + 1,
            }).ToList();

            var model = new LeaveRequestReadOnlyVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = approvedLeaveRequests.Count,
                PendingRequests = pendingLeaveRequests.Count,
                RejectedRequests = rejectedLeaveRequests.Count,
                LeaveRequests = leaveRequestReadOnlyVMs
            };

            return model;
        }

        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);

            if (leaveRequest == null)
            {
                throw new Exception("Leave Request doesn't exist.");
            }

            if (leaveRequest.LeaveRequestStatusId != (int)LeaveRequestStatusCode.Pending &&
                leaveRequest.LeaveRequestStatusId != (int)LeaveRequestStatusCode.Approved)
            {
                return;
            }
       
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusCode.Cancelled;
            
            await ChangeAllocationDays(leaveRequest, decrease: false);
            await _context.SaveChangesAsync();
        }

        public async Task ApproveLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await GetPendingRequestForReview(leaveRequestId);
            if (leaveRequest == null) return;

            var user = await _userService.GetLoggedInUser();
            leaveRequest.ReviewerId = user.Id;
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusCode.Approved;

            await _context.SaveChangesAsync();
        }

        public async Task RejectLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await GetPendingRequestForReview(leaveRequestId);
            if (leaveRequest == null) return;

            var user = await _userService.GetLoggedInUser();
            leaveRequest.ReviewerId = user.Id;
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusCode.Rejected;

            await ChangeAllocationDays(leaveRequest, decrease: false);

            await _context.SaveChangesAsync();
        }



        public async Task<List<EmployeeLeaveRequestListVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userService.GetLoggedInUser();
            var leaveRequests = await _context.LeaveRequests
                .Where(r => r.EmployeeId == user.Id)
                .Include(r => r.LeaveType)
                .ToListAsync();
            var model = leaveRequests.Select(q => new EmployeeLeaveRequestListVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusCode)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber + 1
            })
            .ToList();


            return model;
        }

        public async Task<ReviewLeaveRequestVM> ReviewLeaveRequestAsync(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .FirstOrDefaultAsync(q => q.Id == leaveRequestId); 

            if (leaveRequest == null)
            {
                throw new Exception("Leave Request doesn't exist.");
            }
        
            
            var employeeVm = new ReviewLeaveRequestVM
            {
                LeaveRequestId = leaveRequest.Id,
                userId = leaveRequest.EmployeeId,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                LeaveType = leaveRequest.LeaveType?.Name ?? string.Empty,
                LeaveRequestStatus = (LeaveRequestStatusCode)leaveRequest.LeaveRequestStatusId,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber + 1,
                RequestComments = leaveRequest.RequestComments,
            };

            return employeeVm;
        }


        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestVM model)
        {
            var user = await _userService.GetLoggedInUser();

            var numberOfDays = CountDays(model.StartDate, model.EndDate);

            var period = await _periodsService.GetCurrentPeriod();
    
            if (period == null)
            {
                throw new Exception("No leave period covers the requested dates.");
            }

            var allocation = await _context.LeaveAllocations.FirstAsync(a =>
        a.LeaveTypeId == model.LeaveTypeId &&
        a.EmployeeId == user.Id &&
        a.PeriodId == period.Id);

            return numberOfDays > allocation.NumberOfDays;
        }

        public async Task<bool> PeriodExistsForDates(LeaveRequestVM model)
        {
            return await _context.Periods.AnyAsync(p =>
                p.StartDate <= model.StartDate && model.EndDate <= p.EndDate);
        }

        private async Task<LeaveRequest?> GetPendingRequestForReview(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(q => q.Id == leaveRequestId);

            if (leaveRequest == null)
            {
                throw new Exception("Leave Request doesn't exist.");
            }
            if (leaveRequest.LeaveRequestStatusId != (int)LeaveRequestStatusCode.Pending)
            {
                return null;
            }
            return leaveRequest;
        }

        private async Task<LeaveRequest> ChangeAllocationDays(LeaveRequest leaveRequest, bool decrease)
        {
            var numberOfDays = CountDays(leaveRequest.StartDate, leaveRequest.EndDate);
            var period = await _periodsService.GetCurrentPeriod();
            if (period == null)
            {
                throw new Exception("No leave period covers the requested dates.");
            }

            var allocation = await _leavesAllocationService.GetCurrentAllocation
                (leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);

            if (decrease)
            {
                allocation.NumberOfDays -= numberOfDays;
            }
            else
            {
                allocation.NumberOfDays += numberOfDays;
            }
            return leaveRequest;
        }

        private static int CountDays(DateOnly start, DateOnly end) =>
            end.DayNumber - start.DayNumber + 1;
    }
}
