using static Chat.ServerService;

namespace Chat.Windows;

public class Servers(ITool _tool, ServerService _serverService, IRouter _router) : Window
{
    public override string Name => nameof(Servers);

    public override async Task Open()
    {
        List<Server> result = await _serverService.GetServers();

        foreach (Server item in result)
        {
            _tool.WriteLine($"{item.ServerName}: {item.ServerId}");
        }

        _tool.WriteLine($"{Environment.NewLine}Press ESC to return to the main menu");
        if (_tool.ReadKey().Key == ConsoleKey.Escape)
        {
            _router.Navigate("MainMenu");
        }
    }
}
