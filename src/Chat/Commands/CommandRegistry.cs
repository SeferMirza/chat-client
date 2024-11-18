namespace Chat.Commands;

public class CommandRegistry(IEnumerable<ICommand> _commandsList)
{
    readonly Dictionary<string, ICommand> _commands =
        _commandsList.ToDictionary( x => x.Command, x => x);

    public ICommand? GetCommand(string command)
    {
        _commands.TryGetValue(command, out ICommand? result);

        return result;
    }
}