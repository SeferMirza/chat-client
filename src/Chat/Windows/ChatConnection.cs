namespace Chat.Windows;

public class ChatConnection(ITool _tool, Hub chatHub, IRouter _router) : Window
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
            Guid serverId = Guid.Parse(_tool.ReadLine()!);

            _tool.Write("Username: ");
            string? username = _tool.ReadLine();

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

                if (message.Equals("/exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    _router.Navigate(nameof(MainMenu));
                    break;
                }

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
