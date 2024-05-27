using PricingCalculator.Api.Models;

namespace PricingCalculator.Api.Services
{
    public interface IMessageService
    {
        string GetFailedEndDateStartDateMessage();
        string GetDeserializeFailedMessage();
        string GetSuccessfulMessage(decimal totalPrice, PricingRequestDto requestDto, string serviceName);
    }
}
