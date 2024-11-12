namespace Chat.Windows;

public class ChatConnection(ITool _tool, Hub chatHub) : Window
{
    public override string Name => "Chat";

    public async override Task Open()
    {
        try
        {
            Console.CursorVisible = true;
            chatHub.Subscribe();

            await chatHub.Connect();

            _tool.Write("Server ID: ");
            Guid serverId = Guid.Parse(_tool.ReadLine()!);

            _tool.Write("Username: ");
            string? username = _tool.ReadLine();

            var oldMessages = await chatHub.JoinServer(username!, serverId);
            if (oldMessages is not null)
            {
                foreach (var message in oldMessages)
                {
                    _tool.Write($"[{message.SentAt}] ");
                    if (message.Sender == username)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        _tool.Write($"You: ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        _tool.Write($"{message.Sender}: ");
                    }

                    Console.ResetColor();

                    _tool.Write($"{message.Content}");
                    _tool.WriteLine("");
                }
            }

            while (true)
            {
                string message = _tool.ReadLine() ?? string.Empty;

                if (message.Equals("/exit", StringComparison.CurrentCultureIgnoreCase))
                    break;

                _tool.ClearLine(1);
                if (!message.Equals(string.Empty))
                    await chatHub.SendMessage(serverId, message);
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
