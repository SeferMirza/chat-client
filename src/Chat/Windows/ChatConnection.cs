using Chat.Commands;

namespace Chat.Windows;

public class ChatConnection(ITool _tool, ServerService _serverService, IRouter _router, Hub chatHub, CommandRegistry commandRegistry) : Window
{
    public override string Name => nameof(ChatConnection);

    public async override Task Open()
    {

        Console.CursorVisible = true;

        await chatHub.Connect();

        chatHub.Subscribe();

        List<Message> oldMessages;

        Guid serverId;
        while(true)
        {
            _tool.Write("Server ID: ");
            string input = _tool.ReadLine();
            if(Guid.TryParse(input, out serverId))
            {
                var server = await _serverService.GetServer(serverId);
                if(server != null)
                {
                    break;
                }
                else
                {
                    _tool.ClearLine(1);
                    _tool.WriteLine("Server Cannot Found! Try Again!");
                }
            }
            else
            {
                _tool.ClearLine(1);
                _tool.WriteLine("Invalid Server ID");
            }
        }

        string username = string.Empty;
        while(true)
        {
            _tool.Write("Username (only valid in the server): ");
            username = _tool.ReadLine();
            if(!string.IsNullOrEmpty(username))
            {
                var response = await _serverService.CheckUsername(username, serverId);
                if(response)
                {
                    break;
                }
                else
                {
                    _tool.ClearLine(1);
                    _tool.WriteLine("This username already exist in this room!");
                }
            }
            else
            {
                    _tool.ClearLine(1);
                    _tool.WriteLine("Username cannot be empty!");
            }
        }

        oldMessages = await chatHub.JoinServer(username, serverId);

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
                    await chatHub.SendMessage(serverId, message);
                }
            }
        }

        await chatHub.DisposeAsync();

    }
}
