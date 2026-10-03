using Spectre.Console;
using TUInvaders.Domain;

namespace TUInvaders.Features.Rendering;

public sealed class DrawFrameCommand(GameState gameState, IAnsiConsole console)
    : IDrawFrameCommand
{
    private readonly GameState _gameState = gameState;
    private readonly IAnsiConsole _console = console;

    public void Ececute()
    {
        // Clear screen and cursor to the top left
        _console.Cursor.SetPosition(0, 0);

        // Draw the score to the top using the Spectre.Console tags
        _console.MarkupLine($"[bold yellow] SCORE:[/][white]{_gameState.Score:D4}[/]       [grey]PRESS ESC TO EXIT[/]");
        _console.WriteLine();

        // Create canvas of the active game area
        int canvasHeight = _gameState.ScreenHeight - 3; // 3 rows for leave space to the score
        int canvasWidth = _gameState.ScreenWidth;

        var canvas = new Canvas(canvasWidth, canvasHeight);

        // Aliens rendering (green)
        foreach (var alien in _gameState.Aliens)
        {
            // Check if the alien is inside the canvas
            if (alien.Position.X >= 0 && alien.Position.X < canvasWidth &&
                alien.Position.Y >= 0 && alien.Position.Y < canvasHeight)
            {
                canvas.SetPixel(alien.Position.X, alien.Position.Y, Color.Green);
                // Note: to insert custom chars in the canvas, Spectre use the pixel color
                // If we want complex special chars, Specte allows to insert strings
                // For the classinc canvas we use the color block ot dot
            }
        }

        // Laser rendering
        foreach (var laser in _gameState.Lasers)
        {
            if (laser.Position.X >= 0 && laser.Position.X < canvasWidth &&
                laser.Position.Y >= 0 && laser.Position.Y < canvasHeight)
            {
                var color = laser.IsPlayerLaser ? Color.Red : Color.Yellow;
                canvas.SetPixel(laser.Position.X, laser.Position.Y, color);
            }
        }

        // Player rendering
        var player = _gameState.PlayerPosition;
        if (player.X >= 0 && player.X < canvasWidth &&
            player.Y >= 0 && player.Y < canvasHeight)
        {
            if (player.X > 0) canvas.SetPixel(player.X - 1, player.Y, Color.Cyan);
            canvas.SetPixel(player.X, player.Y, Color.Cyan);
            if (player.X < canvasWidth - 1) canvas.SetPixel(player.X + 1, player.Y, Color.Cyan);
           
            if (player.Y > 0) canvas.SetPixel(player.X, player.Y - 1, Color.Cyan);
        }
        
        // Write of the c=terminal canvas
        _console.Write(canvas);
    }
}