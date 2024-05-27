using Microsoft.Extensions.DependencyInjection;
using PricingCalculator.Service.Interfaces;
using PricingCalculator.Service.Services;

namespace PricingCalculator.Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPriceServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IPricingCalculatorService, PricingCalculatorService>();
    }
}
