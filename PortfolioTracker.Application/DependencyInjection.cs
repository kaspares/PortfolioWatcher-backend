using Microsoft.Extensions.DependencyInjection;
using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Application.Profiles;
using PortfolioTracker.Application.Services;

namespace PortfolioTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPortfolioService, PortfolioService>();
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(PortfolioProfile).Assembly);
        });
        return services;
    }
}
