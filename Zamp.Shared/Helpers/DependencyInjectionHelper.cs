using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Zamp.Shared.Helpers;

public interface IScopedInjectable; // This is a marker interface used to mark scoped dependencies that will automatically be registered (by AddInjectables)

public static class DependencyInjectionHelper
{
    public static IServiceCollection AddInjectables(this IServiceCollection services, Assembly assembly)
    {
        // Automatically register all IScopedInjectable implementations
        foreach (var type in assembly.GetTypes()
                     .Where(t => typeof(IScopedInjectable).IsAssignableFrom(t)
                                 && t is { IsClass: true, IsAbstract: false }))
        {
            services.AddScoped(type);
        }

        return services;
    }
}