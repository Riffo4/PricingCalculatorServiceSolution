namespace PricingCalculator.Service.Domain
{
    public class PricingcalculatorModel
    {
        public decimal? Price { get; private set; }
        public Discount? Discount { get; private set; }
        public DateTime? StartDate { get; private set; }
        public List<int> WorkingDays { get; private set; }
        public PricingcalculatorModel(decimal? price, Discount? discount, DateTime? startDate, List<int>? workingDays = null)
        {
            Price = price;
            Discount = discount;
            StartDate = startDate;
            WorkingDays = workingDays ?? new List<int>() { 1, 2, 3, 4, 5 };
        }
    }
}
