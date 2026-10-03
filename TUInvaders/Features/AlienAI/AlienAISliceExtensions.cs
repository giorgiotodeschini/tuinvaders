using Microsoft.Extensions.DependencyInjection;

namespace TUInvaders.Features.AlienAI;

public static class AlienAISliceExtensions
{
    public static IServiceCollection AddAlienAISlice(this IServiceCollection services)
    {
        services.AddTransient<IUpdateAliensCommand, UpdateAliensCommand>();
        return services;
    }
}