using System.ComponentModel;
using Spectre.Console;
using TUInvaders.Domain;
using TUInvaders.Features.AlienAI;
using TUInvaders.Features.GameOverDisplay;
using TUInvaders.Features.Initialization;
using TUInvaders.Features.PlayerMovement;
using TUInvaders.Features.Rendering;
using TUInvaders.Features.Shooting;
using TUInvaders.Features.WindowResizer;

namespace TUInvaders.Common;

public interface IGameEngine
{
    void Run();
}

public class GameEngine(
    GameState gameState,
    IAnsiConsole console, 
    IInputProvider inputProvider, 
    IInitializeGameCommand initializeGameCommand,
    IDrawFrameCommand drawFrameCommand,
    IMovePlayerCommand movePlayerCommand,
    IPlayerFireLaserCommand playerFireLaserCommand,
    IUpdateLasersCommand updateLasersCommand,
    IUpdateAliensCommand updateAliensCommand,
    IAlienFireLaserCommand alienFireLaserCommand,
    IShowEndScreenCommand showEndScreenCommand,
    IHandleResizeCommand handleResizeCommand)
    : IGameEngine
{
    private readonly GameState _gameState = gameState;
    private readonly IAnsiConsole _console = console;
    private readonly IInputProvider _inputProvider = inputProvider;
    private readonly IInitializeGameCommand _initializeGameCommand = initializeGameCommand;
    private readonly IDrawFrameCommand _drawFrameCommand = drawFrameCommand;
    private readonly IMovePlayerCommand _movePlayerCommand = movePlayerCommand;
    private readonly IPlayerFireLaserCommand _playerFireLaserCommand = playerFireLaserCommand;
    private readonly IUpdateLasersCommand _updateLasersCommand = updateLasersCommand;
    private readonly IUpdateAliensCommand _updateAliensCommand = updateAliensCommand;
    private readonly IAlienFireLaserCommand _alienFireLaserCommand = alienFireLaserCommand;
    private readonly IShowEndScreenCommand _showEndScreenCommand = showEndScreenCommand;
    private readonly IHandleResizeCommand _handleResizeCommand = handleResizeCommand;

    public void Run()
    {
        while (true)
        {
            // Initial game setup
            _console.Clear();
            _initializeGameCommand.Execute();

            // Main game loop
            while (_gameState.IsRunning)
            {
                // Starting time calculation for to keep the FPC constant
                var startTime = DateTime.UtcNow;

                // Handle window resize
                _handleResizeCommand.Execute();

                // Retrieve input (not blocking)
                var pressedKey = _inputProvider.GetLastKey();
                if (pressedKey == ConsoleKey.Escape)
                {
                    _gameState.IsRunning = false;
                    break; // Game exit
                }

                // Logic of game update
                _movePlayerCommand.Execute(pressedKey);
                _playerFireLaserCommand.Execute(pressedKey);

                _updateLasersCommand.Execute();
                _alienFireLaserCommand.Execute();
                _updateAliensCommand.Execute();

                if (_gameState.Aliens.Count == 0)
                {
                    _gameState.IsRunning = false;
                }

                // Rendering
                _drawFrameCommand.Ececute();

                // Frame Rate control
                var frameDuration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                int sleepTime = (int)(_gameState.TargetFrameTimeMs - frameDuration);
                if (sleepTime > 0)
                {
                    Thread.Sleep(sleepTime);
                }
            }

            bool playAgain = _showEndScreenCommand.Execute();
            if (!playAgain)
            {
                break;
            }
        }
    }
}