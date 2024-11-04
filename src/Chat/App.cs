namespace Chat;

public class App
{
    readonly ConsoleTool tool = new();
    public async Task Run()
    {
        var menu = new Menu([
            new("Servers", ServerApi.GetServers),
            new("Join", ChatConnection.Connect)
        ]);

        while (true)
        {
            tool.Write(menu.Draw());
            var input = tool.Read();
            if(input == "/exit")
            {
                break;
            }

            await menu.Action(input);
        }
    }
}
