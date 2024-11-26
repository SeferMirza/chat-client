using Chat.Commands;
using Chat.Consoles;
using Chat.Hubs;

namespace Chat.Windows;

public class ChatConnection(ITool _tool, ServerService _serverService, IRouter _router, ChatHub chatHub, CommandRegistry commandRegistry) : Window
{
    public override string Name => nameof(ChatConnection);

    public async override Task Open()
    {
        Console.CursorVisible = true;

        Guid serverId;
        while (true)
        {
            _tool.Write("Server ID: ");
            string input = _tool.ReadLine();
            if (!Guid.TryParse(input, out serverId))
            {
                _tool.ClearLine(1);
                _tool.WriteLine("Invalid Server ID");
            }
            else
            {
                var servers = await _serverService.GetServers();
                if (servers.Any(s => s.ServerId == serverId))
                {
                    break;
                }
                else
                {
                    _tool.ClearLine(1);
                    _tool.WriteLine("Server not found! Try again.");
                }
            }
        }

        string username = string.Empty;
        while (true)
        {
            _tool.Write("Username (only valid in the server): ");
            username = _tool.ReadLine();

            var response = await _serverService.CheckUsername(username, serverId);
            if (response)
            {
                break;
            }
            else
            {
                _tool.ClearLine(1);
                _tool.WriteLine("This username already exist in this room!");
            }

        }

        await chatHub.Connect();

        chatHub.Subscribe();

        List<Message> oldMessages;

        oldMessages = await chatHub.JoinServer(username, serverId);

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

                await command.ExecuteAsync([$"--serverId {serverId}"]);

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
