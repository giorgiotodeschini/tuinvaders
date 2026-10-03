namespace TUInvaders.Features.PlayerMovement;

public interface IMovePlayerCommand
{
    void Execute(ConsoleKey? key);
}