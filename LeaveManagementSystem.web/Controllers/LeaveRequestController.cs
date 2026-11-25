using LeaveManagementSystem.web.Models.LeaveRequests;
using LeaveManagementSystem.web.Models.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using LeaveManagementSystem.web.Services.LeaveRequests;
using LeaveManagementSystem.web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.web.Controllers;

[Authorize]
public class LeaveRequestController(ILeaveRequestsService leaveRequestsService, ILeaveTypesService leaveTypesService) : Controller
{
    private readonly ILeaveRequestsService _leaveRequestsService = leaveRequestsService;
    private readonly ILeaveTypesService _leaveTypesService = leaveTypesService;


    public async Task<IActionResult> Index()
    {
        var viewData = await _leaveRequestsService.GetEmployeeLeaveRequests();

        return View(viewData);
    }
    public async Task<IActionResult> Create(int? leaveTypeId)
    {
        
        var leaveTypes = await _leaveTypesService.GetAll();
        var leaveTypesList = new SelectList(leaveTypes, "Id", "Name", leaveTypeId);
        var model = new LeaveRequestVM
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            LeaveTypes = leaveTypesList
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LeaveRequestVM leaveRequestVM)
    {
        if (!await _leaveRequestsService.PeriodExistsForDates(leaveRequestVM))
        {
            ModelState.AddModelError(string.Empty, "Leave for those dates isn't open yet. Please contact your manager.");
        }
        else if (await _leaveRequestsService.RequestDatesExceedAllocation(leaveRequestVM))
        {
            ModelState.AddModelError(string.Empty, "The number of days requested is invalid");
            ModelState.AddModelError(nameof(leaveRequestVM.EndDate), "The number of days requested is invalid");
        }

        if (ModelState.IsValid)
        {
            await _leaveRequestsService.CreateLeaveRequest(leaveRequestVM);
            return RedirectToAction(nameof(Index));
        }

        var leaveTypes = await _leaveTypesService.GetAll();
        leaveRequestVM.LeaveTypes = new SelectList(leaveTypes, "Id", "Name");

        return View(leaveRequestVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int leaveRequestId)
    {
        await _leaveRequestsService.CancelLeaveRequest(leaveRequestId);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(policy: "AdminSupervisorOnly")]
    public async Task<IActionResult> ListRequests()
    {
        var leaveRequests = await _leaveRequestsService.GetAllLeaveRequests();
        return View("RequestsList", leaveRequests);
    }

    public async Task<IActionResult> Review(int leaveRequestId)
    {
        var model = await _leaveRequestsService.ReviewLeaveRequestAsync(leaveRequestId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveRequest(int leaveRequestId)
    {
        await _leaveRequestsService.ApproveLeaveRequest(leaveRequestId);
        return RedirectToAction(nameof(ListRequests));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectRequest(int leaveRequestId)
    {
        await _leaveRequestsService.RejectLeaveRequest(leaveRequestId);
        return RedirectToAction(nameof(ListRequests));
    }


    [HttpPost]
    public async Task<IActionResult> Review(/*Use VM*/)
    {
        return View();
    }

}
