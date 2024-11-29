
namespace Chat.Commands;

public class ExitServer(IRouter _router) : ICommand
{
    public string Command => "/exit";

    public void Execute()
    {
        _router.NavigateBack();
    }

    public Task ExecuteAsync()
    {
        Execute();

        return Task.CompletedTask;
    }
}