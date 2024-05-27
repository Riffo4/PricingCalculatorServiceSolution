using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PricingCalculator.Service.Interfaces;
using PricingCalculator.Api.Models;
using PricingCalculator.Api.Services;

namespace PricingCalculator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PricingCalculatorController : ControllerBase
    {
        private readonly ILogger<PricingCalculatorController> _logger;
        private readonly IPricingCalculatorService _pricingService;
        private readonly IMessageService _messageService;

        public PricingCalculatorController(
            ILogger<PricingCalculatorController> logger,
            IPricingCalculatorService pricingService,
            IMessageService messageService)
        {
            _logger = logger;
            _pricingService = pricingService;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("servicea")]
        public string ServiceA(string request)
        {
            decimal totalPrice;
            PricingRequestDto requestDto = null;
            try
            {
                requestDto = JsonConvert.DeserializeObject<PricingRequestDto>(request);
                if (requestDto == null)
                {
                    _logger.LogError("Request invalid.");
                    return _messageService.GetDeserializeFailedMessage();
                }

                if (requestDto.EndDate.HasValue && requestDto.EndDate.Value < requestDto.StartDate)
                {
                    return _messageService.GetFailedEndDateStartDateMessage();
                }

                totalPrice = _pricingService.CalculatePricing(nameof(ServiceA), requestDto.CustomerId, requestDto.StartDate, requestDto.EndDate);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(ServiceA)} failed.");
                return _messageService.GetDeserializeFailedMessage();
            }
            
            return _messageService.GetSuccessfulMessage(totalPrice, requestDto, nameof(ServiceA));
        }

        [HttpGet]
        [Route("serviceb")]
        public string ServiceB(string request)
        {
            decimal totalPrice;
            PricingRequestDto requestDto = null;
            try
            {
                requestDto = JsonConvert.DeserializeObject<PricingRequestDto>(request);
                if (requestDto == null)
                {
                    _logger.LogError("Request invalid.");
                    return _messageService.GetDeserializeFailedMessage();
                }

                totalPrice = _pricingService.CalculatePricing(nameof(ServiceB), requestDto.CustomerId, requestDto.StartDate, requestDto.EndDate);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(ServiceB)} failed.");
                return _messageService.GetDeserializeFailedMessage();
            }

            return _messageService.GetSuccessfulMessage(totalPrice, requestDto, nameof(ServiceB));
        }

        [HttpGet]
        [Route("servicec")]
        public string ServiceC(string request)
        {
            decimal totalPrice;
            PricingRequestDto requestDto = null;
            try
            {
                requestDto = JsonConvert.DeserializeObject<PricingRequestDto>(request);
                if (requestDto == null)
                {
                    _logger.LogError("Request invalid.");
                    return _messageService.GetDeserializeFailedMessage();
                }

                totalPrice = _pricingService.CalculatePricing(nameof(ServiceC), requestDto.CustomerId, requestDto.StartDate, requestDto.EndDate);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(ServiceC)} failed.");
                return _messageService.GetDeserializeFailedMessage();
            }

            return _messageService.GetSuccessfulMessage(totalPrice, requestDto, nameof(ServiceC));
        }       
    }
}
