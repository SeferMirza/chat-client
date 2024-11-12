namespace Chat;

public class App(WindowStack windowStack, ITool _tool)
{
    public async Task Run()
    {
        while (true)
        {
            _tool.ClearFull();
            await windowStack.Current.Open();
        }
    }
}
