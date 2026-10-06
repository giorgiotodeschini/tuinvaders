using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using TUInvaders.Common;
using TUInvaders.Common.Audio;
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

        var audioService = new SharpAudioService();
        audioService.RegisterSound(Sounds.PlayerLaser, "Assets/player_laser.wav");
        audioService.RegisterSound(Sounds.AlienLaser, "Assets/alien_laser.wav");
        audioService.RegisterSound(Sounds.Explosion, "Assets/explosion.wav");

        // Dependency Injection Container configuration
        using var serviceProvider = new ServiceCollection()
            .AddSingleton(new GameState(initialWidth, initialHeight)) // Global state
            // Common infrastructure
            .AddSingleton<IAnsiConsole>(AnsiConsole.Console)
            .AddSingleton<IGameEngine, GameEngine>()
            .AddSingleton<IInputProvider, InputProvider>()
            .AddSingleton<IAudioService>(audioService)
            .AddInitializationSlice()
            .AddPlayerMovementSlice()
            .AddAlienAISlice()
            .AddShootingSlice()
            .AddRenderigSlice()
            .AddGameOverSlice()
            .AddWindowResizerSlice()
            .BuildServiceProvider();
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
            AnsiConsole.Cursor.Show();
        }
    }
}