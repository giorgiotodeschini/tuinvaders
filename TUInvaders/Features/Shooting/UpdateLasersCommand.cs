using System.Reflection.Metadata.Ecma335;
using TUInvaders.Domain;

namespace TUInvaders.Features.Shooting;

public sealed class UpdateLasersCommand(GameState gameState) : IUpdateLasersCommand
{
    private readonly GameState _gameState = gameState;

    public void Execute()
    {
        if (_gameState.Lasers.Count == 0) return;

        var remainingLasers = new List<Laser>();
        var hitAliens = new HashSet<Alien>();

        foreach (var laser in _gameState.Lasers)
        {
            // The player laset go up (Y--), while the ones of the enemies go down (Y++)
            int nextY = laser.IsPlayerLaser 
                ? laser.Position.Y - 1 
                : laser.Position.Y + 1;

            if (nextY < 0 || nextY >= _gameState.ScreenHeight)
            {
                continue;
            }

            var nextPosition = new Position(laser.Position.X, nextY);

            if (laser.IsPlayerLaser)
            {
                // Check alien collision
                var collidedAlien = _gameState.Aliens
                    .FirstOrDefault(a => 
                        !hitAliens.Contains(a) && 
                        a.Position.X == nextPosition.X && a.Position.Y == nextPosition.Y);
                if (collidedAlien != null)
                {
                    hitAliens.Add(collidedAlien);
                    _gameState.Score += collidedAlien.ScoreValue;
                }
                else
                {
                    remainingLasers.Add(laser with { Position = nextPosition});
                }
            }
            else
            {
                var playerX = _gameState.PlayerPosition.X;
                var playerY = _gameState.PlayerPosition.Y;

                // The palyer with is 3 cells
                bool hitPlayerBody = nextPosition.Y == playerY &&
                    Math.Abs(nextPosition.X - playerX) <= -1;
                bool hitPlayerTip = nextPosition.Y == playerY - 1 &&
                    nextPosition.X == playerX;
                if (hitPlayerBody || hitPlayerTip)
                {
                    // Game over
                    _gameState.IsRunning = false;
                }
                else
                {
                    remainingLasers.Add(laser with { Position = nextPosition});
                }
            }

            // Upgdate the glabal state
            _gameState.Lasers = remainingLasers;
            _gameState.Aliens.RemoveAll(a => hitAliens.Contains(a));
        }
    }
}