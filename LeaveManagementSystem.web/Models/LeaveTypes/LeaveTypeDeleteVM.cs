using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.web.Models.LeaveTypes
{
    public class LeaveTypeDeleteVM : BaseLeaveTypeVM
    {
        public string Name { get; set; }
        [Display(Name = "Max Days")]
        public int Days { get; set; }
    }


}
