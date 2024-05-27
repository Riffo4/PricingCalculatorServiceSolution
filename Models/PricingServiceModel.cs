namespace Models
{
    public class PricingServiceModel
    {
        public decimal? Price { get; set; }
        public Discount? Discount { get; set; }
        public DateTime? StartDate { get; set; }
        public List<int> WorkingDays { get; set; }
        public PricingServiceModel()
        {
            Price = null;
            Discount = null;
            StartDate = null;
            WorkingDays = new List<int>() { 1, 2, 3, 4, 5 };
        }
    }
}
