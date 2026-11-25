using LeaveManagementSystem.web.Models.LeaveTypes;

namespace LeaveManagementSystem.web.Services.LeaveTypes
{
    public interface ILeaveTypesService
    {
        
        Task Create(LeaveTypeCreateVM model);
        Task Edit(LeaveTypeEditVM model);
        Task<T?> Get<T>(int id) where T : class;
        Task<List<LeaveTypeReadOnlyVM>> GetAll();
        bool LeaveTypeExists(int? id);
        Task<bool> CheckIfLeaveTypeNameExists(string name);
        Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit);
        Task Remove(int id);
        Task<bool> UpdateChecker(int leaveTypeId, int days);

    }
}