using Spectre.Console;
using TUInvaders.Domain;

namespace TUInvaders.Features.GameOverDisplay;

public sealed class ShowEndScreenCommand(GameState gameState, IAnsiConsole console)
    : IShowEndScreenCommand
{
    private readonly GameState _gameState = gameState;
    private readonly IAnsiConsole _console = console;

    public bool Execute()
    {
        _console.Clear();

        // Chech if the payer wins
        var isVictory = _gameState.Aliens.Count == 0 && _gameState.IsRunning == false;
        if (isVictory)
        {
            _console.Write(new FigletText("YOU WIN!").Color(Color.Green).Centered());
        }
        else
        {
            _console.Write(new FigletText("GAME OVER").Color(Color.Red).Centered());
        }

        _console.WriteLine();

        // Creating a score panel
        var scoreMarkup = $"[bold]Final Score:[/][yellow]{_gameState.Score:D4}[/]";
        //var panel = new Panel(new Align(new Text(scoreMarkup), HorizontalAlignment.Center))
        var panel = new Panel(new Align(new Markup(scoreMarkup), HorizontalAlignment.Center))
        {
            Border = BoxBorder.Rounded,
            Padding = new (2, 1, 2, 1),
            Header = new PanelHeader("[bold white] STATISTICS[/]", Justify.Center)
        };

        _console.Write(new Padder(panel).Padding(0, 1, 0, 1));
        _console.WriteLine();

        var choice = _console.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold cyan]What do you want to do?[/]")
                .PageSize(3)
                .AddChoices(["Restart", "Exit"]));
        
        return choice == "Restart";
    }
}