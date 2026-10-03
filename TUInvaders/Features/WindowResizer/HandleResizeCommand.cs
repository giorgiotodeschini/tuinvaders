using Spectre.Console;
using TUInvaders.Domain;

namespace TUInvaders.Features.WindowResizer;

public sealed class HandleResizeCommand(GameState gameState, IAnsiConsole console)
    : IHandleResizeCommand
{
    private readonly GameState _gameState = gameState;
    private readonly IAnsiConsole _console = console;

    public void Execute()
    {
        int currentWidth = _console.Profile.Width;
        int currentHeight = _console.Profile.Height;

        // If the size was not changed exit
        if (currentWidth == _gameState.ScreenWidth && currentHeight == _gameState.ScreenHeight)
        {
            return;
        }

        // Save the old size to calculate the scale ratio
        double scaleX = (double)currentWidth / _gameState.ScreenWidth;
        double scaleY = (double)currentHeight / _gameState.ScreenHeight;

        _gameState.UpdateDimensions(currentWidth, currentHeight);

        // Set the new player position
        int newPlayerX = (int)Math.Clamp(_gameState.PlayerPosition.X * scaleX, 1, currentWidth - 2);
        _gameState.PlayerPosition = new (newPlayerX, currentHeight - 4);

        // Set the remaining aliens new position
        for (int i = 0; i < _gameState.Aliens.Count; i++)
        {
            var alien = _gameState.Aliens[i];
            int newAlienX = (int)Math.Clamp(alien.Position.X * scaleX, 1, currentWidth - 2);
            int newAlienY = (int)Math.Clamp(alien.Position.Y * scaleY, 2, currentHeight - 5);

            _gameState.Aliens[i] = alien with { Position = new (newAlienX, newAlienY) };
        }

        // Clean lasers
        _gameState.Lasers.Clear();

        // Clear screen
        _console.Clear();
    }
}