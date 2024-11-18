
using Chat.Windows;

namespace Chat.Commands;

public class ExitServer(IRouter _router) : ICommand
{
    public string Command => "/exit";

    public void Execute(string[]? args = default)
    {
        _router.Navigate(nameof(MainMenu));
    }

    public Task ExecuteAsync(string[]? args = default)
    {
        Execute();

        return Task.CompletedTask;
    }
}