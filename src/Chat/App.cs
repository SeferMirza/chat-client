namespace Chat;

public class App(IWindowManager windowManager, ITool _tool)
{
    public async Task Run()
    {
        while (true)
        {
            _tool.ClearFull();
            await windowManager.OpenCurrentWindow();
        }
    }
}
