namespace TUInvaders.Common;

public interface IInputProvider
{
    ConsoleKey? GetLastKey();
}

public class InputProvider : IInputProvider
{
    public ConsoleKey? GetLastKey()
    {
        // If there isn't key pressed return null without to block
        if (!Console.KeyAvailable)
        {
            return null;
        }

        // Read the key and clear the console buffer
        var keyInfo = Console.ReadKey(intercept: true);

        // Empty keys pressed too speedly
        while (Console.KeyAvailable)
        {
            Console.ReadKey(intercept: true);
        }

        return keyInfo.Key;
    }
}