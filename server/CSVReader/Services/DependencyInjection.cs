using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Services.Implementations;

namespace Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddContactService();
        return services;
    }

    private static IServiceCollection AddContactService(this IServiceCollection services)
    {
        services.AddScoped<IContactService, ContactService>();
        return services;
    }
}
