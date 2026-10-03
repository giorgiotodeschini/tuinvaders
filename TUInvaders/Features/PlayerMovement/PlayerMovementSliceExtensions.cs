using Microsoft.Extensions.DependencyInjection;

namespace TUInvaders.Features.PlayerMovement;

public static class PlayerMovementSliceExtensions
{
    public static IServiceCollection AddPlayerMovementSlice(this IServiceCollection services)
    {
        services.AddTransient<IMovePlayerCommand, MovePlayerCommand>();
        return services;
    }
}