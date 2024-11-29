namespace Chat.Commands;

public interface ICommand
{
    string Command { get; }
    void Execute();
    Task ExecuteAsync();
}
