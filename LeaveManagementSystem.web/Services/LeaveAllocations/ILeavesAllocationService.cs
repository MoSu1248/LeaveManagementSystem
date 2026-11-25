using LeaveManagementSystem.web.Models.LeaveAllocations;

namespace LeaveManagementSystem.web.Services.LeaveAllocations
{
    public interface ILeavesAllocationService
    {
        Task AllocateLeave(string employeeId);
        //Task<List<LeaveAllocation>> GetAllocations( string? userId);
        Task<EmployeeAllocationVM> GetEmployeeAllocation(string? userId);
        Task<LeaveAllocationEditVM> EditEmployeeAllocation(int allocationId);
        Task<List<EmployeeListVM>> GetEmployees();
        Task EditAllocation(LeaveAllocationEditVM allocationEditVM);
        Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId);

    }
}
