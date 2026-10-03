namespace TUInvaders.Features.Shooting;

public interface IPlayerFireLaserCommand
{
    void Execute(ConsoleKey? key);
}