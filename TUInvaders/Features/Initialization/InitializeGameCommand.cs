using TUInvaders.Domain;

namespace TUInvaders.Features.Initialization;

public sealed class InitializeGameCommand(GameState gameState) 
    : IInitializeGameCommand
{
    private readonly GameState _gameState = gameState;
    
    public void Execute()
    {
        _gameState.Aliens.Clear();
        _gameState.Lasers.Clear();
        _gameState.IsRunning = true;
        _gameState.Score = 0;

        // Dynamic parametrization based the the terminal size
        int startY = 2; // Start from the 3 row from the top to reserve space to the score indicator
        int rowsCount = 4; // Number of rows of aliens
        int alienSpacingX = 4; // Space between aliens
        int marginX = (int)(_gameState.ScreenWidth * 0.2); // Left margin 20%
        int availableWidth = (int)(_gameState.ScreenWidth * 0.6); // 60%

        int aliensPerRow = Math.Max(3, availableWidth / alienSpacingX);

        for (int row = 0; row < rowsCount; row++)
        {
            int currentY = startY + (row * 2); // Interline 1 row
            int scoreValue = (rowsCount - row) * 10; // Upper aliens have higher value

            for (var col = 0; col < aliensPerRow; col++)
            {
                int currentX = marginX + (col * alienSpacingX);

                // Avoid that the last alien exceed the right margin
                if (currentX < _gameState.ScreenWidth - 2)
                {
                    _gameState.Aliens.Add(new Alien(new(currentX, currentY), scoreValue));
                }
            }
        }
    }
}