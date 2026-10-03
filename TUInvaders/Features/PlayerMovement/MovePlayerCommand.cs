using TUInvaders.Domain;

namespace TUInvaders.Features.PlayerMovement;

public class MovePlayerCommand(GameState gameState) : IMovePlayerCommand
{
    private readonly GameState _gameState = gameState;

    public void Execute(ConsoleKey? key)
    {
        if (key == null) return;

        var currentX = _gameState.PlayerPosition.X;
        var currentY = _gameState.PlayerPosition.Y;

        switch (key)
        {
            case ConsoleKey.LeftArrow:
            case ConsoleKey.A:
                if (currentX > 1)
                {
                    _gameState.PlayerPosition = new(currentX - 1, currentY);
                }
                break;

            case ConsoleKey.RightArrow:
            case ConsoleKey.D:
                if (currentX < _gameState.ScreenWidth - 2)
                {
                    _gameState.PlayerPosition = new(currentX + 1, currentY);
                }
                break;
        }
    }
}