using LeaveManagementSystem.web.Models.LeaveRequests;

namespace LeaveManagementSystem.web.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequest(LeaveRequestVM model);

        Task<List<EmployeeLeaveRequestListVM>> GetEmployeeLeaveRequests();
        Task<LeaveRequestReadOnlyVM> GetAllLeaveRequests();

        Task CancelLeaveRequest(int leaveRequestId);

        Task<bool> RequestDatesExceedAllocation(LeaveRequestVM model);
        Task<ReviewLeaveRequestVM> ReviewLeaveRequestAsync(int leaveRequestId);

        Task ApproveLeaveRequest(int leaveRequestId);
        Task RejectLeaveRequest(int leaveRequestId);
        Task<bool> PeriodExistsForDates(LeaveRequestVM model);

        
    }
}