using TUInvaders.Domain;

namespace TUInvaders.Features.Shooting;

public class PlayerFireLaserCommand(GameState gameState) : IPlayerFireLaserCommand
{
    private readonly GameState _gameState = gameState;
    private const int MaxPlayerLasers = 3;

    public void Execute(ConsoleKey? key)
    {
        if (key != ConsoleKey.Spacebar) return;

        int activePlayerLasers = _gameState.Lasers.Count(l => l.IsPlayerLaser);
        if (activePlayerLasers < MaxPlayerLasers)
        {
            var spawnPosition = new Position(_gameState.PlayerPosition.X, 
                _gameState.PlayerPosition.Y - 2);
            _gameState.Lasers.Add(new(spawnPosition, IsPlayerLaser: true));
        }
    }
}