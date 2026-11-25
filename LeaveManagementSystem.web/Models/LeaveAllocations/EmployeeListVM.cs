using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.web.Models.LeaveAllocations
{
    public class EmployeeListVM
    {
        public string userId { get; set; } = string.Empty;

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}
