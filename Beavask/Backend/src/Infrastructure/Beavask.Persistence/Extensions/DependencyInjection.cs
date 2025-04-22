using Beavask.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Service;

namespace Beavask.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddServices();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<UserRepository>() 
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<IUserContactService>() 
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
