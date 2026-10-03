using Microsoft.Extensions.DependencyInjection;

namespace TUInvaders.Features.Shooting;

public static class ShootingSliceExtensions
{
    public static IServiceCollection AddShootingSlice(this IServiceCollection services)
    {
        services.AddTransient<IPlayerFireLaserCommand, PlayerFireLaserCommand>();
        services.AddTransient<IUpdateLasersCommand, UpdateLasersCommand>();
        services.AddTransient<IAlienFireLaserCommand, AlienFireLaserCommand>();
        return services;
    }
}