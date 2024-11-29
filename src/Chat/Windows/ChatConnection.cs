using Chat.Commands;
using Chat.Consoles;
using Chat.Exceptions;
using Chat.Hubs;

namespace Chat.Windows;

public class ChatConnection(ITool _tool, ServerService _serverService, IRouter _router, ChatHub chatHub, CommandRegistry commandRegistry) : Window
{
    private int _currentIndex;

    public override string Name => nameof(ChatConnection);

    public async override Task Open()
    {
        _tool.WriteLine("Username: ");
        string username = _tool.ReadLine();

        await chatHub.Connect();

        chatHub.Subscribe();

        List<Message> oldMessages;

        oldMessages = await chatHub.JoinServer(username, _serverService.CurrentServer?.ServerId ?? throw new ServerConnectionException());

        await Task.Delay(500);

        if (oldMessages is not null)
        {
            foreach (var message in oldMessages)
            {
                _tool.WriteChatMessage(message, username);
            }
        }

        while (true)
        {
            string message = _tool.ReadLine() ?? string.Empty;

            var command = commandRegistry.GetCommand(message);
            if (command != null)
            {
                _tool.ClearLine(1);

                await command.ExecuteAsync();

                if (_router.Path != nameof(ChatConnection))
                {
                    break;
                }
            }
            else
            {
                if (!message.Equals(string.Empty) && command == null)
                {
                    _tool.ClearLine(1);
                    await chatHub.SendMessage(message);
                }
            }
        }

        await chatHub.DisposeAsync();
    }
}
