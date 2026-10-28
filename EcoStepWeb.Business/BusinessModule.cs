using EcoStepWeb.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EcoStepWeb.Business;

public static class BusinessModule
{
    public static IServiceCollection AddBusinessModule(this IServiceCollection services)
    {
        services.AddScoped<IActivityService, ActivityService>();

        return services;
    }
}
