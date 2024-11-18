namespace Chat.Commands;

public interface ICommand
{
    string Command { get; }
    void Execute(params string[] args);
    Task ExecuteAsync(params string[] args);
}
