namespace LeaveManagementSystem.web.Services.Periods
{
    public interface IPeriodsService
    {
      
        Task<Period> GetCurrentPeriod();

        Task<bool> PeriodExists(DateOnly startDate, DateOnly endDate, int? excludePeriodId = null);

    }
}
