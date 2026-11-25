
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.web.Data;
using LeaveManagementSystem.web.Services.Periods;

[Authorize(Roles = Roles.Administrator)]

public class PeriodsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IPeriodsService _periodsService;
    public PeriodsController(ApplicationDbContext context, IPeriodsService periodsService)
    {
        _context = context;
        _periodsService = periodsService;
    }

    // GET: PERIODS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Periods.ToListAsync());
    }

    // GET: PERIODS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var period = await _context.Periods
            .FirstOrDefaultAsync(m => m.Id == id);
        if (period == null)
        {
            return NotFound();
        }

        return View(period);
    }

    // GET: PERIODS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PERIODS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,StartDate,EndDate,Id")] Period period)
    {
        if (!ModelState.IsValid)
        {
            return View(period);
        }

        var periodExists = await _periodsService.PeriodExists(period.StartDate, period.EndDate);

        if (periodExists)
        {
            ModelState.AddModelError(string.Empty,
           "This period overlaps an existing period.");
            return View(period);
        }
    
        _context.Add(period);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
       
    }

    // GET: PERIODS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var period = await _context.Periods.FindAsync(id);
        if (period == null)
        {
            return NotFound();
        }
        return View(period);
    }

    // POST: PERIODS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Name,StartDate,EndDate,Id")] Period period)
    {
        if (id != period.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var overlaps = await _periodsService.PeriodExists(
                period.StartDate,
                period.EndDate,
                period.Id);

            if (overlaps)
            {
                ModelState.AddModelError(string.Empty,
                    "This period overlaps an existing period.");
                return View(period);
            }

            try
            {
                _context.Update(period);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeriodExists(period.Id))
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
        return View(period);
    }

    // GET: PERIODS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var period = await _context.Periods
            .FirstOrDefaultAsync(m => m.Id == id);
        if (period == null)
        {
            return NotFound();
        }

        return View(period);
    }

    // POST: PERIODS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var period = await _context.Periods.FindAsync(id);
        if (period != null)
        {
            _context.Periods.Remove(period);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PeriodExists(int? id)
    {
        return _context.Periods.Any(e => e.Id == id);
    }
}
