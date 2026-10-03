using Microsoft.Extensions.DependencyInjection;

namespace TUInvaders.Features.WindowResizer;

public static class WindowResizerSliceExtensions
{
    public static IServiceCollection AddWindowResizerSlice(this IServiceCollection services)
    {
        services.AddTransient<IHandleResizeCommand, HandleResizeCommand>();
        return services;
    }
}