namespace PricingCalculator.Service.Domain
{
    public class CustomerInfo
    {
        public Dictionary<string, PricingcalculatorModel> PricingServices { get; private set; }
        public int FreeDays { get; private set; }
        public string Name { get; private set; }
        public CustomerInfo(Dictionary<string, PricingcalculatorModel>  pricingServices, int freeDays, string name)
        {
            PricingServices = pricingServices;
            FreeDays = freeDays;
            Name = name;
        }
    }
}
