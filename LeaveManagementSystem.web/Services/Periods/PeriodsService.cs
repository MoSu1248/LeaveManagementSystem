using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.web.Services.Periods
{
    public class PeriodsService(ApplicationDbContext _context) : IPeriodsService
    {
        public async Task<Period> GetCurrentPeriod()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Today);
            var period = await _context.Periods.SingleOrDefaultAsync(p => p.StartDate <= currentDate
            && p.EndDate >= currentDate);
            return period;
        }

        public async Task<bool> PeriodExists(
            DateOnly startDate,
            DateOnly endDate,
            int? excludePeriodId = null)
        {
            return await _context.Periods.AnyAsync(p =>
                p.StartDate <= endDate &&
                startDate <= p.EndDate &&
                (!excludePeriodId.HasValue || p.Id != excludePeriodId.Value));
        }
    }
}
