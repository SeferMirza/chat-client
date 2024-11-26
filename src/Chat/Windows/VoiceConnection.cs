using Chat.Consoles;
using Chat.Hubs;
using static Chat.ServerService;

namespace Chat.Windows;

public class VoiceConnection(ServerService _server, VoiceHub voiceHub, ITool _tool, IRouter _router) : Window
{
    public override string Name => nameof(VoiceConnection);
    int _currentIndex = 0;
    Server? _currentServer = null;

    public async override Task Open()
    {
        List<Server> servers = await _server.GetVoiceServers();

        while(true)
        {
            _tool.ClearFull();

            for (int i = 0; i < servers.Count; i++)
            {
                string serverShowLine = $"{servers[i].ServerName}: {servers[i].ServerId}";

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
                _router.Navigate(nameof(MainMenu));

                break;
            }
            else if(consoleKey == ConsoleKey.Enter)
            {
                _currentServer = servers[_currentIndex];
                break;
            }
        }

        if(_currentServer == null)
        {
            return;
        }

        _tool.Write("Username: ");
        var username = _tool.ReadLine();

        await voiceHub.Connect(_currentServer.ServerId, username);

        do
        {
            _tool.ClearFull();
            _tool.WriteLine("To exit press 'Escape' key!");
        } while(_tool.ReadKey().Key != ConsoleKey.Escape);

        await voiceHub.DisposeAsync();
        _router.Navigate(nameof(MainMenu));
    }
}
