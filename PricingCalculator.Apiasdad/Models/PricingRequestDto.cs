namespace PricingCalculator.Api.Models
{
    public class PricingRequestDto
    {
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public PricingRequestDto() { }
    }
}
