using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.web.Models.LeaveRequests
{
    public class LeaveRequestVM : IValidatableObject
    {
        [Required]
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }
        [Required]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }
        [Display(Name = "Additional Comments")]
        [MaxLength(250, ErrorMessage = "Comments cannot exceed 250 characters.")]
        public string? RequestComments { get; set; }

        public SelectList? LeaveTypes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(StartDate > EndDate)
            {
                yield return new ValidationResult("Start Date cannot be later than End Date.", new[] { nameof(StartDate), nameof(EndDate) });
            }
        }
    }
}