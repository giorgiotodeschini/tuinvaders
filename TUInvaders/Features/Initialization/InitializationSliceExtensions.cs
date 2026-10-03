using Microsoft.Extensions.DependencyInjection;

namespace TUInvaders.Features.Initialization;

public static class InitializationSliceExtensions
{
    public static IServiceCollection AddInitializationSlice(this IServiceCollection services)
    {
        services.AddTransient<IInitializeGameCommand, InitializeGameCommand>();
        return services;
    }
}