using Microsoft.Extensions.DependencyInjection;

namespace TUInvaders.Features.GameOverDisplay;

public static class GameOverSliceExtensions
{
    public static IServiceCollection AddGameOverSlice(this IServiceCollection services)
    {
        services.AddTransient<IShowEndScreenCommand, ShowEndScreenCommand>();
        return services;
    }
}