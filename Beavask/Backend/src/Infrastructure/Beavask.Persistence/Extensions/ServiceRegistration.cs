using Beavask.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Beavask.Persistence.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<UserRepository>() 
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
