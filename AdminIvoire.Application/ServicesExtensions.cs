using Microsoft.Extensions.DependencyInjection;

namespace AdminIvoire.Application;

public static class ServicesExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ServicesExtensions).Assembly;

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(assembly);
        });

        return services;
    }
}
