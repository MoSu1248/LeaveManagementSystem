using AutoMapper;
using LeaveManagementSystem.web.Models.LeaveAllocations;
using LeaveManagementSystem.web.Services.Periods;
using LeaveManagementSystem.web.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.web.Services.LeaveAllocations
{
    public class LeavesAllocationService: ILeavesAllocationService
    {

        private readonly ApplicationDbContext _context;
   
 
        private readonly IMapper _mapper;       
        private readonly IPeriodsService _periodsService;

        private readonly IUserService _userService;

        public LeavesAllocationService(ApplicationDbContext context, IUserService userService, IMapper mapper, IPeriodsService periodsService)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _periodsService = periodsService;
        }

        public async Task AllocateLeave(string employeeId)
        {
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(a => a.EmployeeId == employeeId))
                .ToListAsync();
                
                
            var currentDate = DateTime.Now;
            var period = await _periodsService.GetCurrentPeriod();
            var monthsRemaining = period.EndDate.Month - currentDate.Month;

            foreach (var leaveType in leaveTypes)
            {
              
                var accuralRate = decimal.Divide(leaveType.NumberOfDays, 12);
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    NumberOfDays = (int)Math.Ceiling(accuralRate * monthsRemaining)
                };

                _context.Add(leaveAllocation);
            }

            await _context.SaveChangesAsync();
        }
        
        public async Task<EmployeeAllocationVM> GetEmployeeAllocation(string? userId)
        {
            var user = string.IsNullOrEmpty(userId) 
                ? await _userService.GetLoggedInUser()
                : await _userService.GetUserById(userId);

            var allocations = await GetAllocations(user.Id);
            var allocationVMList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationsVM>>(allocations);
            var leaveTypesCount = await _context.LeaveTypes.CountAsync();
            var employeeVm = new EmployeeAllocationVM
            {
                userId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                LeaveAllocations = allocationVMList,
                IsCompletedAllocations = allocations.Count == leaveTypesCount
            };


            return employeeVm;
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var employees = await _userService.GetEmployees();
            var employeeListVMs = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(employees);
            return employeeListVMs; 
        }

        public async Task EditAllocation(LeaveAllocationEditVM allocationEditVM)
        {
            
            var rowsUpdated = await _context.LeaveAllocations
                .Where(q=> q.Id == allocationEditVM.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.NumberOfDays, allocationEditVM.NumberOfDays));

            if (rowsUpdated == 0)
            {
                throw new Exception("Leave allocation record does not exist.");
            }
        }

        public async Task<LeaveAllocationEditVM> EditEmployeeAllocation(int allocationId)
        {
            var allocation = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id == allocationId);

            var model = _mapper.Map<LeaveAllocationEditVM>(allocation);

            return model;
        }

        public async Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId)
        {
            var period = await _periodsService.GetCurrentPeriod();
            var allocation = await _context.LeaveAllocations
                .FirstOrDefaultAsync(q => q.LeaveTypeId == leaveTypeId && q.EmployeeId == employeeId && q.PeriodId == period.Id);
            return allocation;
        }

        private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
        {

            var user = await _userService.GetLoggedInUser();
            var currentDate = DateTime.Now;

            if (user == null)
            {
                return new List<LeaveAllocation>();
            }

            var leaveAllocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == userId && q.Period.EndDate.Year == currentDate.Year)
                .ToListAsync();

            return leaveAllocations;
        }


        private async Task<bool> AllocationExists(string userId, int leaveTypeId, int periodId)
        {
            var exists = await _context.LeaveAllocations.AnyAsync(q => q.EmployeeId == userId && q.LeaveTypeId == leaveTypeId && q.PeriodId == periodId);
            return exists;
        }


      

    }

  


}
