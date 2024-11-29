
namespace Chat.Commands;

public class ExitServer(IRouter _router) : ICommand
{
    public string Command => "/exit";

    public void Execute(string[]? args = default)
    {
        _router.NavigateBack();
    }

    public Task ExecuteAsync(string[]? args = default)
    {
        Execute();

        return Task.CompletedTask;
    }
}