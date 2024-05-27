namespace Models
{
    public class Discount
    {
        public decimal DiscountAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Discount(decimal discountAmount, DateTime? startDate = null, DateTime? endDate = null) 
        {
            DiscountAmount = discountAmount;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
