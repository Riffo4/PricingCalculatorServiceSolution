namespace PricingCalculator.Service.Domain
{
    public class Discount
    {
        public decimal DiscountAmount { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public Discount(decimal discountAmount, DateTime? startDate = null, DateTime? endDate = null) 
        {
            DiscountAmount = discountAmount;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
