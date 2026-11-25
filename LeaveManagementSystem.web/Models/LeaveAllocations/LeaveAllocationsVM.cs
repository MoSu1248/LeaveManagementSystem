using LeaveManagementSystem.web.Models.LeaveAllocations.Periods;
using LeaveManagementSystem.web.Models.LeaveTypes;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.web.Models.LeaveAllocations
{
    public class LeaveAllocationsVM
    {
        public int Id { get; set; }

        [Display(Name = "Number of days")]
        public int NumberOfDays { get; set; }
        [Display(Name = "Allocation Period")]
        public PeriodVM Period { get; set; } = new PeriodVM();
        public LeaveTypeReadOnlyVM? LeaveType { get; set; }
    }
}
