using PricingCalculator.Service.Interfaces;
using PricingCalculator.Service.Domain;

namespace PricingCalculator.Service.Services
{
    public class PricingCalculatorService : IPricingCalculatorService
    {
        private readonly Dictionary<int, CustomerInfo> _customerData;
        private readonly Dictionary<string, PricingcalculatorModel> _baseCostPricingServices;

        public PricingCalculatorService()
        {
            _customerData = GetCustomers();
            _baseCostPricingServices = GetBaseCostPricingServices();
        }        

        public decimal CalculatePricing(string serviceName, int customerId, DateTime startDate, DateTime? endDate)
        {
            if (!_customerData.ContainsKey(customerId))
            {
                throw new KeyNotFoundException("Customer not found");
            }

            _customerData.TryGetValue(customerId, out var customerInfo);
            decimal totalPrice = 0m;
            var addFreeDays = customerInfo.FreeDays > 0;
            var freeDays = customerInfo.FreeDays;
            endDate = endDate ?? new DateTime(2020, 6, 1);//DateTime.Today;

            var pricingServicesDictionary = customerInfo.PricingServices.Where(x => x.Value.StartDate != null).ToList();

            if (!pricingServicesDictionary.Exists(x => x.Key == serviceName))
            {
                pricingServicesDictionary.Add(_baseCostPricingServices.FirstOrDefault(x => x.Key == serviceName));
            }

            foreach (var pricingServiceDictionary in pricingServicesDictionary)
            {
                var pricingServiceValue = pricingServiceDictionary.Value;
                    
                var baseCostPricingService = _baseCostPricingServices.FirstOrDefault(x => x.Key == pricingServiceDictionary.Key).Value;
                var workingDays = baseCostPricingService.WorkingDays;

                var price = pricingServiceValue.Price.HasValue ? pricingServiceValue.Price.Value : baseCostPricingService.Price.Value;
                var discount = pricingServiceValue.Discount;

                var currentDate = startDate;
                var newFreeDays = freeDays;
                if (serviceName != pricingServiceDictionary.Key && pricingServiceValue.StartDate > startDate)
                {
                    // Use the start date of the customers pricingservice when a different service is called
                    // And startDate is later than the start date input
                    var freeDaysReduceAmount = pricingServiceValue.StartDate - startDate;
                    newFreeDays = freeDays - freeDaysReduceAmount.Value.Days;
                    currentDate = pricingServiceValue.StartDate.Value;                    
                }
                var index = 0;
                while (currentDate <= endDate)
                {
                    if (addFreeDays)
                    {
                        addFreeDays = false;
                        currentDate = currentDate.AddDays(newFreeDays);
                        continue;
                    }

                    if (!workingDays.Contains((int)currentDate.DayOfWeek))
                    {
                        currentDate = currentDate.AddDays(1);
                        continue;
                    }

                    totalPrice += AddDailyPrice(currentDate, price, discount);                    

                    currentDate = currentDate.AddDays(1);
                    index++;
                }
                addFreeDays = customerInfo.FreeDays > 0;
            }

            return Math.Round(totalPrice, 2);
        }

        private static decimal AddDailyPrice(DateTime currentDate, decimal price, Discount? discount)
        {
            if (discount != null && discount?.DiscountAmount > 0)
            {
                if (discount.StartDate.HasValue)
                {
                    if (discount.StartDate.Value <= currentDate && currentDate <= discount.EndDate.Value)
                    {
                        price = price * discount.DiscountAmount;                        
                    }
                }
                else
                {
                    // Add discount even if discount doesnt have any start date                    
                    price = price * discount.DiscountAmount;
                }
            }            

            return Math.Round(price, 2);
        }

        public int GetAmountOfWorkingDays(DateTime startDate, DateTime endDate, bool addFreeDays, int freeDays, List<int> workingDays)
        {
            var index = 0;
            while (startDate <= endDate)
            {
                if (addFreeDays)
                {
                    addFreeDays = false;
                    startDate = startDate.AddDays(freeDays);
                    continue;
                }

                if (!workingDays.Contains((int)startDate.DayOfWeek))
                {
                    startDate = startDate.AddDays(1);
                    continue;
                }

                startDate = startDate.AddDays(1);
                index++;
            }

            return index;
        }
 
        private static Dictionary<string, PricingcalculatorModel> GetBaseCostPricingServices()
        {
            return new Dictionary<string, PricingcalculatorModel>
            {
                { "ServiceA", new PricingcalculatorModel((decimal?)0.2, null, null, new List<int>() { 1, 2, 3, 4, 5 }) },
                { "ServiceB", new PricingcalculatorModel((decimal?)0.24, null, null, new List<int>() { 1, 2, 3, 4, 5 }) },
                { "ServiceC", new PricingcalculatorModel((decimal?)0.4, null, null, new List<int>() { 0, 1, 2, 3, 4, 5, 6 }) }
            };
        }

        private Dictionary<int, CustomerInfo> GetCustomers()
        {
            // Sample data representing customer pricing information
            return new Dictionary<int, CustomerInfo>
            {
                { 0, new CustomerInfo(new Dictionary<string, PricingcalculatorModel>
                        {
                            { "ServiceA", new PricingcalculatorModel((decimal?)0.15, null, new DateTime(2019, 9, 20), null) },
                            { "ServiceC", new PricingcalculatorModel((decimal?)0.25, new Discount((decimal)0.8, new DateTime(2019, 09, 22), new DateTime(2019,09, 24)), new DateTime(2019, 9, 20), null) },
                        },
                        0,
                        "CustomerX"
                    )
                },
                { 1, new CustomerInfo(new Dictionary<string, PricingcalculatorModel>
                        {
                            { "ServiceB", new PricingcalculatorModel(null, new Discount((decimal)0.7), new DateTime(2018, 1, 1)) },
                            { "ServiceC", new PricingcalculatorModel(null, new Discount((decimal)0.7), new DateTime(2018, 1, 1)) },
                        },
                        200,
                        "CustomerY"
                    )
                }
            };
        }
    }
}
