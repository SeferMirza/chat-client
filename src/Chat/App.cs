using Chat.Consoles;
using Chat.Exceptions;

namespace Chat;

public class App(IWindowManager windowManager, ITool _tool, IExceptionHandler exceptionHandler)
{
    public async Task Run()
    {
        while (true)
        {
            try
            {
                _tool.ClearFull();
                await windowManager.OpenCurrentWindow();
            }
            catch(Exception ex)
            {
                exceptionHandler.HandleException(ex);
            }
        }
    }
}
