using PricingCalculator.Service.Services;

namespace Tests
{
    public class PricingCalculatorServiceUnitTest
    {
        private readonly PricingCalculatorService _pricingService;

        public PricingCalculatorServiceUnitTest()
        {
            _pricingService = new PricingCalculatorService();
        }

        // The customers involved
        //    {
        //        { 0, new CustomerInfo(new Dictionary<string, PricingServiceModel>
        //                {
        //                    { "ServiceA", new PricingServiceModel((decimal?)0.15, null, new DateTime(2019, 9, 20), null) },
        //                    { "ServiceC", new PricingServiceModel((decimal?)0.25, new Discount((decimal)0.8, new DateTime(2019-09-22), new DateTime(2019-09-24)), new DateTime(2019, 9, 20), null) },
        //                },
        //                0,
        //                "CustomerX"
        //            )
        //        },
        //        { 1, new CustomerInfo(new Dictionary<string, PricingServiceModel>
        //                {
        //                    { "ServiceB", new PricingServiceModel(null, new Discount((decimal)0.7), new DateTime(2018, 1, 1)) },
        //                    { "ServiceC", new PricingServiceModel(null, new Discount((decimal)0.7), new DateTime(2018, 1, 1)) },
        //                },
        //                200,
        //                "CustomerY"
        //            )
        //        }
        //    }

        // The base costs
        //{ "ServiceA", new PricingServiceModel((decimal?)0.2, null, null, new List<int>() { 1, 2, 3, 4, 5 }) },
        //{ "ServiceB", new PricingServiceModel((decimal?)0.24, null, null, new List<int>() { 1, 2, 3, 4, 5 }) },
        //{ "ServiceC", new PricingServiceModel((decimal?)0.4, null, null, new List<int>() { 0, 1, 2, 3, 4, 5, 6 }) }

        [Fact]
        public void CalculatePricing_TestCase1_FromAssignment_ShouldCalculateServiceAAndServiceCBetweenSpecficedDates()
        {
            RefactoredTestFunctionToProveBothTestsWorksTC1("ServiceA");
        }        

        [Fact]
        public void CalculatePricing_TestCase1_FromAssignment_ShouldCalculateServiceAAndServiceCBetweenSpecficedDates_ShouldNotMatterUsingServiceC()
        {
            RefactoredTestFunctionToProveBothTestsWorksTC1("ServiceC");
        }

        private void RefactoredTestFunctionToProveBothTestsWorksTC1(string serviceName)
        {
            // Arrange
            int customerId = 0;
            DateTime startDate = new DateTime(2019, 9, 20);
            DateTime? endDate = new DateTime(2019, 10, 01);

            // Act
            decimal result = _pricingService.CalculatePricing(serviceName, customerId, startDate, endDate);

            // Assert
            decimal expectedPrice = 0.15m * 8 + (0.25m * 9 + 0.25m * 0.8m * 3); // (Service A) 8 working days (excluding weekends) + (Service C) 12 working days (including weekends) with 20% discount for 3 days
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        public void CalculatePricing_TestCase2_FromAssignment_ShouldCalculateServiceBAndServiceCBetweenSpecficedDatesWith200FreeDaysAndForTheRestOfTheTime()
        {
            RefactoredTestFunctionToProveBothTestsWorksTC2("ServiceB");
        }

        [Fact]
        public void CalculatePricing_TestCase2_FromAssignment_ShouldCalculateServiceBAndServiceCBetweenSpecficedDatesWith200FreeDaysAndForTheRestOfTheTime_ShouldNotMatterUsingServiceC()
        {
            RefactoredTestFunctionToProveBothTestsWorksTC2("ServiceC");
        }

        private void RefactoredTestFunctionToProveBothTestsWorksTC2(string serviceName)
        {
            // Arrange
            int customerId = 1;
            DateTime startDate = new DateTime(2018, 01, 01);
            DateTime? endDate = new DateTime(2019, 10, 01);

            // Act
            decimal result = _pricingService.CalculatePricing(serviceName, customerId, startDate, endDate);

            // Assert
            decimal expectedPrice = (0.17m * 313) + (0.4m * 0.7m * 439);
            // (Service B) 313 working days (excluding weekends) with discount of 30% + 
            // (Service C) 439 working days (including weekends) with 30% discount 0.4 * 0.7 = 0.28 * 438 = 122.64
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        private void CalculateAmountOfWorkingDaysBetweenTwoDates_WithServiceBWorkingDays()
        {
            // Arrange
            DateTime startDate = new DateTime(2018, 01, 01);
            DateTime endDate = new DateTime(2019, 10, 01);

            // Act
            int result = _pricingService.GetAmountOfWorkingDays(startDate, endDate, true, 200, new List<int>() { 1, 2, 3, 4, 5 });            
            
            // Assert
            decimal expectedPrice = 313;
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        private void CalculateAmountOfWorkingDaysBetweenTwoDates_WithServiceCWorkingDays()
        {
            // Arrange
            DateTime startDate = new DateTime(2018, 01, 01);
            DateTime endDate = new DateTime(2019, 10, 01);

            // Act
            int result = _pricingService.GetAmountOfWorkingDays(startDate, endDate, true, 200, new List<int>() { 0, 1, 2, 3, 4, 5, 6 });

            // Assert
            decimal expectedPrice = 439;
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        public void CalculatePricing_InvalidCustomerId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            string serviceName = "ServiceA";
            int customerId = 999;
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime? endDate = new DateTime(2020, 1, 10);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _pricingService.CalculatePricing(serviceName, customerId, startDate, endDate));
        }

        [Fact]
        public void CalculatePricing_ValidCustomerNoFreeDays_ShouldCalculateCorrectPriceForServiceAFor8WorkingDays()
        {
            // Arrange
            string serviceName = "ServiceA";
            int customerId = 0;
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime? endDate = new DateTime(2020, 1, 10);

            // Act
            decimal result = _pricingService.CalculatePricing(serviceName, customerId, startDate, endDate);

            // Assert
            decimal expectedPrice = 0.15m * 8 + 0.25m * 10; // (Service A) 8 working days (excluding weekends) + (Service C) 10 working days (include weekends)
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        public void CalculatePricing_ValidCustomerWithFreeDays_ShouldSkipFreeDays()
        {
            // Arrange
            string serviceName = "ServiceA";
            int customerId = 1;
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime? endDate = new DateTime(2020, 1, 10);

            // Act
            decimal result = _pricingService.CalculatePricing(serviceName, customerId, startDate, endDate);

            // Assert
            decimal expectedPrice = 0; // 200 free days
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        public void CalculatePricing_WithDiscount_ShouldApplyDiscountCorrectly()
        {
            // Arrange
            string serviceName = "ServiceC";
            int customerId = 0;
            DateTime startDate = new DateTime(2019, 9, 22);
            DateTime? endDate = new DateTime(2019, 9, 24);

            // Act
            decimal result = _pricingService.CalculatePricing(serviceName, customerId, startDate, endDate);

            // Assert
            decimal serviceACost = 0.15m * 2;
            decimal discountedPrice = 0.25m * 0.8m; // 20% discount
            decimal expectedPrice = discountedPrice * 3 + serviceACost; // 3 days (22nd, 23rd, 24th)
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        public void CalculatePricing_WithBaseCost_ShouldUseBaseCostIfNoCustomerSpecificPricing()
        {
            // Arrange
            string serviceName = "ServiceB";
            int customerId = 0;
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime? endDate = new DateTime(2020, 1, 10);

            // Act
            decimal result = _pricingService.CalculatePricing(serviceName, customerId, startDate, endDate);

            // Assert
            decimal expectedPrice = 0.15m * 8 + 0.25m * 10 + 0.24m * 8; // 8 working days (excluding weekends)
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        public void CalculatePricing_WithDiscountAndFreeDays_ShouldApplyBothCorrectly()
        {
            // Arrange
            string serviceName = "ServiceC";
            int customerId = 1;
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime? endDate = new DateTime(2020, 1, 10);

            // Act
            decimal result = _pricingService.CalculatePricing(serviceName, customerId, startDate, endDate);

            // Assert
            decimal expectedPrice = 0m; // All days are free due to the customer's FreeDays
            Assert.Equal(expectedPrice, result);
        }
    }
}