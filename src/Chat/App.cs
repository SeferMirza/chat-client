namespace Chat;

public class App(WindowStack windowStack, ITool _tool)
{
    public async Task Run()
    {
        _tool.WriteLine("Continuation will result in console clear! Use the (C) key to continue. Press another key to exit.");
        var key = _tool.ReadKey().Key;
        if(key == ConsoleKey.C)
        {
            while ( true )
            {
                _tool.ClearFull();
                await windowStack.Current.Open();
            }
        }
    }
}
