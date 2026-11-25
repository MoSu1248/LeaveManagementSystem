using LeaveManagementSystem.web.Services.LeaveRequests;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.web.Models.LeaveRequests
{
    public class EmployeeLeaveRequestListVM
    {
        public int Id { get; set; }

        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }

        [Display(Name = "Leave Type")]
        public string LeaveType { get; set; } = string.Empty;

        [Display(Name = "Number of Days")]
        public int NumberOfDays { get; set; }

        [Display(Name = "Status")]
        public LeaveRequestStatusCode LeaveRequestStatus { get; set; }

    }
}