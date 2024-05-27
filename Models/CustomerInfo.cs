namespace Models
{
    public class CustomerInfo
    {
        public Dictionary<string, PricingServiceModel> PricingServices { get; set; }
        public int FreeDays { get; set; }
        public string Name { get; set; }
    }
}
