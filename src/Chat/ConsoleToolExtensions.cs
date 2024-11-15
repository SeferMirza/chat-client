namespace Chat;

public static class ConsoleToolExtensions
{
    public static void WriteChatMessage(this ITool _tool, Message message, string? sender)
    {
        _tool.Write($"[{message.SentAt}] ");
        if (message.Sender == sender)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            _tool.Write($"You: ");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            _tool.Write($"{message.Sender}: ");
        }

        Console.ResetColor();

        _tool.Write($"{message.Content}");
        _tool.WriteLine("");
    }
}