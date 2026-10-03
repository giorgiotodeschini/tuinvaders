namespace TUInvaders.Domain;

public record Position(int X, int Y);

public record Alien(Position Position, int ScoreValue, string Symbol = "👾");

public record Laser(Position Position, bool IsPlayerLaser);

public class GameState(int screenWidth, int screenHeight)
{
    public int ScreenWidth { get; } = screenWidth;
    public int ScreenHeight { get; } = screenHeight;

    // Game loop status
    public bool IsRunning { get; set; } = true;
    public int Score { get; set; } = 0;
    public int TargetFrameTimeMs { get; } = 33; // ~30 FPS

    // Game entities
    public Position PlayerPosition { get; set; } = new(screenWidth / 2, screenHeight - 4);
    public List<Alien> Aliens { get; set; } = [];
    public List<Laser> Lasers { get; set; } = [];
}