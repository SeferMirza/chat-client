namespace Chat;

public class Option : IOption
{
    readonly Action? _syncAction = null;
    readonly Func<Task>? _asyncAction = null;
    public string Name { get; }

    public Option(string name, Action syncAction)
    {
        Name = name;
        _syncAction = syncAction;
    }

    public Option(string name, Func<Task> asyncAction)
    {
        Name = name;
        _asyncAction = asyncAction;
    }

    public async Task ExecuteAsync()
    {
        if (_asyncAction != null)
        {
            await _asyncAction();
        }
        else if (_syncAction != null)
        {
            _syncAction();
        }
        else
        {
            throw new InvalidOperationException("No operation provided for this option.");
        }
    }
}

public interface IOption
{
    Task ExecuteAsync();
}
