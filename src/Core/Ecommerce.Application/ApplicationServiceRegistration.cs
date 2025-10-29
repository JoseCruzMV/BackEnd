using Ecommerce.Application.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddAplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddServiceEmail(configuration);

        return services;   
    }
}