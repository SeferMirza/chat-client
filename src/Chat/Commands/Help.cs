using Chat.Consoles;

namespace Chat.Commands;

public class Help(ITool _tool) : ICommand
{
    public string Command => "/help";

    public void Execute(params string[] args)
    {
        var help = "To see all command.";
        var info = "To see server info.";
        var exit = "To exit chat.";

        Console.ForegroundColor = ConsoleColor.Magenta;
        _tool.WriteLine("╔" + new string('═', 48) + "╗");
        _tool.Write("║");
        Console.ForegroundColor = ConsoleColor.Green;
        _tool.Write(" '/help': ");
        Console.ForegroundColor = ConsoleColor.White;
        _tool.Write(help);
        Console.ForegroundColor = ConsoleColor.Magenta;
        _tool.Write(new string(' ', 48 - 9 - help.Length - 1) + "║" + Environment.NewLine);

        Console.ForegroundColor = ConsoleColor.Magenta;
        _tool.Write("║");
        Console.ForegroundColor = ConsoleColor.Green;
        _tool.Write(" '/info': ");
        Console.ForegroundColor = ConsoleColor.White;
        _tool.Write(help);
        Console.ForegroundColor = ConsoleColor.Magenta;
        _tool.Write(new string(' ', 48 - 9 - info.Length - 1) + "║" + Environment.NewLine);

        Console.ForegroundColor = ConsoleColor.Magenta;
        _tool.Write("║");
        Console.ForegroundColor = ConsoleColor.Green;
        _tool.Write(" '/exit': ");
        Console.ForegroundColor = ConsoleColor.White;
        _tool.Write(exit);
        Console.ForegroundColor = ConsoleColor.Magenta;
        _tool.Write(new string(' ', 48 - 9 - exit.Length - 1) + "║" + Environment.NewLine);

        _tool.WriteLine("╚" + new string('═', 48) + "╝");

        Console.ResetColor();
    }

    public Task ExecuteAsync(params string[] args)
    {
        Execute();

        return Task.CompletedTask;
    }
}
