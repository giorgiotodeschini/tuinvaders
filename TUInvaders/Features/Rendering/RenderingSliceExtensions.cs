using Microsoft.Extensions.DependencyInjection;

namespace TUInvaders.Features.Rendering;

public static class RenderingSliceExtensions
{
    public static IServiceCollection AddRenderigSlice(this IServiceCollection services)
    {
        services.AddTransient<IDrawFrameCommand, DrawFrameCommand>();
        return services;
    }
}