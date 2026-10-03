using TUInvaders.Domain;

namespace TUInvaders.Features.AlienAI;

public sealed class UpdateAliensCommand(GameState gameState) : IUpdateAliensCommand
{
    private readonly GameState _gameState = gameState;
    private int _frameCount = 0;
    private int _directionX = 1; // 1 = right; -1 = left
    private const int MoveEveryNFrames = 10; // aliens speed

    public void Execute()
    {
        if (_gameState.Aliens.Count == 0) return;

        _frameCount++;
        // Move the aliens only each N frames
        if (_frameCount < MoveEveryNFrames) return;
        _frameCount = 0;

        bool changeDirection = false;

        // Check if one alien touch the borders the next movement
        foreach (var alien in _gameState.Aliens)
        {
            int nextX = alien.Position.X + _directionX;
            if (nextX <= 0 || nextX >= _gameState.ScreenWidth - 1)
            {
                changeDirection = true;
                break;
            }
        }

        var updatedAliens = new List<Alien>();

        if (changeDirection)
        {
            // Revert the direction and go down of 1 row
            _directionX *= -1;

            foreach (var alien in _gameState.Aliens)
            {
                int nextY = alien.Position.Y + 1;

                // Check the game over
                if (nextY > _gameState.PlayerPosition.Y)
                {
                    _gameState.IsRunning = false;
                }

                updatedAliens.Add(alien with { Position = new(alien.Position.X, nextY)});
            }
        }
        else
        {
            // Standard horizontal movement
            foreach (var alien in _gameState.Aliens)
            {
                int nextX = alien.Position.X + _directionX;
                updatedAliens.Add(alien with { Position = new (nextX, alien.Position.Y)});
            }
        }

        _gameState.Aliens = updatedAliens;
    }
}