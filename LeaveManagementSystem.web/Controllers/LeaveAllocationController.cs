using LeaveManagementSystem.web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;
using LeaveManagementSystem.web.Models.LeaveAllocations;
using LeaveManagementSystem.web.Services.LeaveTypes;

namespace LeaveManagementSystem.web.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeavesAllocationService _leavesAllocationService , ILeaveTypesService _leaveTypesService) : Controller
    {
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
            var employeeAllocation = await _leavesAllocationService.GetEmployees();
            return View(employeeAllocation);
        }
        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AllocateLeave(string? userId)
        {
           await _leavesAllocationService.AllocateLeave(userId);
            return RedirectToAction(nameof(Index), new {userId});
        }
        [Authorize(Roles = Roles.Administrator)]
        
        public async Task<IActionResult> EditAllocation(int? allocationId)
        {
            if(allocationId == null)
            {
                return NotFound();
            }
            var allocation = await _leavesAllocationService.EditEmployeeAllocation(allocationId.Value);
            if(allocation == null)
            {
                return NotFound();
            }
            return View(allocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocation)
        {
            if (await _leaveTypesService.UpdateChecker(allocation.LeaveType.Id,allocation.NumberOfDays))
            {
                ModelState.AddModelError("NumberOfDays", "The allocation exceeds the max leave type value");
                return View(allocation);
            }
            if (ModelState.IsValid)
            {
                await _leavesAllocationService.EditAllocation(allocation);
                return RedirectToAction(nameof(Details), new { userId = allocation.Employee?.userId, });
            }

      
            return View(allocation);
          
        }


        public async Task<IActionResult> Details(string? userId)
        {
            var employeeAllocation = await _leavesAllocationService.GetEmployeeAllocation(userId);
            return View(employeeAllocation);
        }
    }
}
