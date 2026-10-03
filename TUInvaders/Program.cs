using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using TUInvaders.Common;
using TUInvaders.Domain;
using TUInvaders.Features.AlienAI;
using TUInvaders.Features.GameOverDisplay;
using TUInvaders.Features.Initialization;
using TUInvaders.Features.PlayerMovement;
using TUInvaders.Features.Rendering;
using TUInvaders.Features.Shooting;
using TUInvaders.Features.WindowResizer;

namespace TUInvaders;

internal class Program
{
    private static void Main(string[] args)
    {
        // Initial terminal setup with Spectre.Console
        AnsiConsole.Profile.Capabilities.Ansi = true;
        AnsiConsole.Cursor.Hide();

        // Retrieve the current dimensions for the initialization
        var initialWidth = AnsiConsole.Console.Profile.Width;
        var initialHeight = AnsiConsole.Console.Profile.Height;

        // Dependency Injection Container configuration
        var serviceProvider = new ServiceCollection()
            .AddSingleton(new GameState(initialWidth, initialHeight)) // Global state
            // Common infrastructure
            .AddSingleton<IAnsiConsole>(AnsiConsole.Console)
            .AddSingleton<IGameEngine, GameEngine>()
            .AddSingleton<IInputProvider, InputProvider>()
            .AddInitializationSlice()
            .AddPlayerMovementSlice()
            .AddAlienAISlice()
            .AddShootingSlice()
            .AddRenderigSlice()
            .AddGameOverSlice()
            .AddWindowResizerSlice()
            .BuildServiceProvider();

        // Game Engine resolution and starting of the game llop
        try
        {
            var gameEngine = serviceProvider.GetRequiredService<IGameEngine>();
            gameEngine.Run();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
        }
        finally
        {
            // Restore cursor when the game is finished
            AnsiConsole.Cursor.Show();
        }
    }
}