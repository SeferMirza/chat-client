using Chat.Exceptions;

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

    public static void WriteHandledExceptionMessage(this ITool _tool, HandledException exception)
    {
        switch (exception.Level)
        {
            case ExceptionLevel.Error:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                _tool.Write($"[ERROR] {exception.Title}: ");
                Console.ForegroundColor = ConsoleColor.Red;
                _tool.Write(exception.Message);
                break;

            default:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                _tool.Write($"[WARNING] {exception.Title}: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                _tool.Write(exception.Message);
                break;
        }
        _tool.WriteLine(string.Empty);
        Console.ResetColor();
    }

    public static void WriteUnhandledExceptionMessage(this ITool _tool, Exception exception)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        _tool.Write("[ERROR] Unhandled Exception: ");
        Console.ForegroundColor = ConsoleColor.Red;
        _tool.Write("Something went wrong. Please report the problem!");
        Console.ResetColor();
    }
}