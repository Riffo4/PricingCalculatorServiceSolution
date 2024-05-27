namespace PricingCalculator.Service.Interfaces
{
    public interface IPricingCalculatorService
    {
        decimal CalculatePricing(string serviceName, int customerId, DateTime startDate, DateTime? endDate);
        int GetAmountOfWorkingDays(DateTime startDate, DateTime endDate, bool addFreeDays, int freeDays, List<int> workingDays);
    }
}
