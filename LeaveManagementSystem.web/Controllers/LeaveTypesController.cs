
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.web.Data;
using LeaveManagementSystem.web.Models.LeaveTypes;
using LeaveManagementSystem.web.Services;

    [Authorize(Roles = Roles.Administrator)]
public class LeaveTypesController(ILeaveTypesService leaveTypesService) : Controller
{
    private string NameExistsValidationMessage = "this leave type with this name already exists.";
    private readonly ILeaveTypesService _leaveTypesService = leaveTypesService;


    // GET: LEAVETYPES
    public async Task<IActionResult> Index()
    {
        var viewData = await _leaveTypesService.GetAll();
        return View(viewData);
    }

    // GET: LEAVETYPES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var leaveType = await _leaveTypesService.Get<LeaveTypeReadOnlyVM>(id.Value);
        if (leaveType == null)
        {
            return NotFound();
        }

        return View(leaveType);
    }

    // GET: LEAVETYPES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: LEAVETYPES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreateVM)
    {

        if (await _leaveTypesService.CheckIfLeaveTypeNameExists(leaveTypeCreateVM.Name))
        {
            ModelState.AddModelError(nameof(leaveTypeCreateVM.Name), NameExistsValidationMessage);
        }

        if (ModelState.IsValid)
        {
            await _leaveTypesService.Create(leaveTypeCreateVM);

            return RedirectToAction(nameof(Index));
        }
        return View(leaveTypeCreateVM);
    }





    // GET: LEAVETYPES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var leaveType = await _leaveTypesService.Get<LeaveTypeEditVM>(id.Value);
        if (leaveType == null)
        {
            return NotFound();
        }
        return View(leaveType);
    }

    // POST: LEAVETYPES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, LeaveTypeEditVM leaveTypeEdit)
    {
        if (id != leaveTypeEdit.Id)
        {
            return NotFound();
        }

        if (await _leaveTypesService.CheckIfLeaveTypeNameExistsForEdit(leaveTypeEdit))
        {
            ModelState.AddModelError(nameof(leaveTypeEdit.Name), NameExistsValidationMessage);
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _leaveTypesService.Edit(leaveTypeEdit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_leaveTypesService.LeaveTypeExists(leaveTypeEdit.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        return View(leaveTypeEdit);
    }

    // GET: LEAVETYPES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var leaveType = await _leaveTypesService.Get<LeaveTypeDeleteVM>(id.Value);
        if (leaveType == null)
        {
            return NotFound();
        }
        return View(leaveType);
    }

    // POST: LEAVETYPES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        await _leaveTypesService.Remove(id.Value);
        return RedirectToAction(nameof(Index));
    }


}
