using Chat.Consoles;

namespace Chat.Windows;

public class Servers(ITool _tool, ServerService _serverService, IRouter _router) : Window
{
    public override string Name => nameof(Servers);
    int _currentIndex = 0;
    Server? _currentServer = null;
    public override async Task Open()
    {
        List<Server> servers = await _serverService.GetServers();

        while(true)
        {
            _tool.ClearFull();

            for (int i = 0; i < servers.Count; i++)
            {
                string publicFlag = servers[i].Public ? "Public" : "Private";
                string serverShowLine = $"{servers[i].ServerName}: {servers[i].ServerId}, {servers[i].ServerType}, {publicFlag}";

                if (i == _currentIndex)
                    _tool.WriteLine($"> {serverShowLine}");
                else
                    _tool.WriteLine($"  {serverShowLine}");
            }

            ConsoleKey consoleKey = _tool.ReadKey().Key;
            if(consoleKey == ConsoleKey.DownArrow) _currentIndex = (_currentIndex + 1) % servers.Count;
            else if(consoleKey == ConsoleKey.UpArrow) _currentIndex = (_currentIndex - 1 + servers.Count) % servers.Count;
            else if(consoleKey == ConsoleKey.Escape)
            {
                Environment.Exit(0);
                break;
            }
            else if(consoleKey == ConsoleKey.Enter)
            {
                _serverService.JoinServer(servers[_currentIndex]);
                _router.Navigate(servers[_currentIndex].ServerType.ToString());
                break;
            }
        }
    }
}
