using System.IO.Pipes;
using System.Reflection.Metadata.Ecma335;
using TUInvaders.Domain;

namespace TUInvaders.Features.Shooting;

public sealed class AlienFireLaserCommand(GameState gamestate) 
    : IAlienFireLaserCommand
{
    private readonly GameState _gameState = gamestate;
    private readonly Random _random = new();
    private const double FireProbability = 0.02; // 2%

    public void Execute()
    {
        if (_gameState.Aliens.Count == 0) return;

        // Check the casuality factor of the current frame
        if (_random.NextDouble() > FireProbability) return;

        // Founf the aliens in the first row
        var shootingCandidates = _gameState.Aliens
            .GroupBy(a => a.Position.X)
            .Select(g => g.OrderByDescending(a => a.Position.Y).First())
            .ToList();

        if (shootingCandidates.Count > 0)
        {
            // Select a random alien from the candidates
            var shooter = shootingCandidates[_random.Next(shootingCandidates.Count)];

            // The enemy laser spawn under the alien
            var spawnPosition  = new Position(shooter.Position.X, shooter.Position.Y + 1);
            _gameState.Lasers.Add(new (spawnPosition, IsPlayerLaser: false));
        }
    }
}