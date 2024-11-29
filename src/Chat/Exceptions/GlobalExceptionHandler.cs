using Chat.Consoles;
using Chat.Windows;

namespace Chat.Exceptions;

public class GlobalExceptionHandler(ITool _tool, IRouter _router) : IExceptionHandler
{
    public void HandleException(Exception exception)
    {
        if(exception is HandledException handledException)
        {
            _tool.WriteHandledExceptionMessage(handledException);
            _tool.WriteLine("Press 'ESC' to exit, 'M' for main menu, press another key to continue.");
            var key = _tool.ReadKey().Key;
            if (key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
            else if (key == ConsoleKey.M)
            {
                _router.NavigateRoot();
            }
            else
            {
                _tool.ClearLine(1);
            }
        }
        else
        {
            _tool.WriteUnhandledExceptionMessage(exception);
            Environment.Exit(0);
        }
    }
}
