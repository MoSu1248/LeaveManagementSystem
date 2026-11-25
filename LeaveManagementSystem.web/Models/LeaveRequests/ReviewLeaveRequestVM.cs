using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.web.Models.LeaveRequests
{
    public class ReviewLeaveRequestVM
    {
        public string userId { get; set; } = string.Empty;
        public int LeaveRequestId { get; set; }

        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }

        [Display(Name = "Leave Type")]
        public string LeaveType { get; set; } = string.Empty;

        [Display(Name = "Number of Days")]
        public int NumberOfDays { get; set; }

        [Display(Name = "Request Comments")]
        public string? RequestComments { get; set; }

        [Display(Name = "Leave Request Status")]
        public LeaveRequestStatusCode LeaveRequestStatus { get; set; }

    }
}