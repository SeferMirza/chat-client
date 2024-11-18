using Chat.Commands;

namespace Chat;

public static class CommandExtensions
{
    public static bool IsCommand(this string text, CommandRegistry commandRegistry) =>
        commandRegistry.GetCommand(text) != null;
}