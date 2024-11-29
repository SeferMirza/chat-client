using Chat.Consoles;
using Chat.Exceptions;
using Chat.Hubs;

namespace Chat.Windows;

public class VoiceConnection(ServerService _server, VoiceHub voiceHub, ITool _tool, IRouter _router) : Window
{
    public override string Name => nameof(VoiceConnection);

    public async override Task Open()
    {
        _tool.Write("Username: ");
        var username = _tool.ReadLine();

        await voiceHub.Connect(_server.CurrentServer?.ServerId ?? throw new ServerConnectionException(), username);

        do
        {
            _tool.ClearFull();
            _tool.WriteLine("To exit press 'Escape' key!");
        } while(_tool.ReadKey().Key != ConsoleKey.Escape);

        await voiceHub.DisposeAsync();
        _router.NavigateBack();
    }
}
