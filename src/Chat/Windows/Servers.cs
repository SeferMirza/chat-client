using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Windows;

public class Servers(ITool _tool, HttpClient _client, IServiceProvider provider) : Window
{
    public override string Name => "Servers";
    const string ServerUrl = "http://localhost:5181/chat";
    Lazy<WindowStack> _windowStack = new(() => provider.GetRequiredService<WindowStack>());

    record ServerInfo(Guid ServerId, string ServerName);
    public override async Task Open()
    {
        try
        {
            var url = Path.Join(ServerUrl, "/servers");
            HttpResponseMessage response = await _client.GetAsync(url);
            List<ServerInfo> result = await response.Content.ReadFromJsonAsync<List<ServerInfo>>() ?? [];

            foreach(ServerInfo item in result)
            {
                _tool.WriteLine($"{item.ServerName}: {item.ServerId}");
            }

            _tool.WriteLine($"{Environment.NewLine}Press ESC to return to the main menu");
            if(_tool.ReadKey().Key == ConsoleKey.Escape)
            {
                _windowStack.Value.Navigate("MainMenu");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }
}
