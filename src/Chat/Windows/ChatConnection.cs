using Chat.Commands;

namespace Chat.Windows;

public class ChatConnection(ITool _tool, IRouter _router, Hub chatHub, CommandRegistry commandRegistry) : Window
{
    public override string Name => nameof(ChatConnection);

    public async override Task Open()
    {
        try
        {
            Console.CursorVisible = true;

            await chatHub.Connect();

            chatHub.Subscribe();

            _tool.Write("Server ID: ");
            Guid serverId = Guid.Parse(_tool.ReadLine()!); // TODO

            _tool.Write("Username: ");
            string? username = _tool.ReadLine(); // TODO

            var oldMessages = await chatHub.JoinServer(username!, serverId);
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
        }
        catch (Exception ex)
        {
            _tool.WriteLine($"Hata: {ex.Message} - {ex.InnerException!.Message}");
        }
        finally
        {
            await chatHub.DisposeAsync();
        }
    }
}
