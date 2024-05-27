using PricingCalculator.Api.Models;

namespace PricingCalculator.Api.Services
{
    public class MessageService : IMessageService
    {
        public string GetFailedEndDateStartDateMessage()
        {
            return $"request failed. \n Correct the start date and end date";
        }

        public string GetDeserializeFailedMessage()
        {
            return $"request failed to deserialize. \n Use this template: {{\"CustomerId\":0,\"StartDate\":\"2019-05-23\",\"EndDate\":\"2020-01-01\"}}";
        }

        public string GetSuccessfulMessage(decimal totalPrice, PricingRequestDto requestDto, string serviceName)
        {
            return $"Total price: {totalPrice} of {serviceName}. Customer id: {requestDto.CustomerId}. Dates: {requestDto.StartDate.ToString("yyyy-MM-dd")} - {requestDto.EndDate?.ToString("yyyy-MM-dd")}";
        }
    }
}
