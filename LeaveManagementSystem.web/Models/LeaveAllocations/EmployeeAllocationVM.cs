using System.ComponentModel.DataAnnotations;
using LeaveManagementSystem.web.Data;

namespace LeaveManagementSystem.web.Models.LeaveAllocations
{
    public class EmployeeAllocationVM : EmployeeListVM
    {
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        public bool IsCompletedAllocations { get; set; }

        public List<LeaveAllocationsVM> LeaveAllocations { get; set; } 
    }
}
