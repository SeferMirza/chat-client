namespace Chat.Windows;

public class ChatConnection(ITool _tool) : Window
{
    public override string Name => "Chat";

    public async override Task Open()
    {
        var chatHub = new Hub();
        try
        {
            chatHub.Subscribe();

            await chatHub.Connect();

            _tool.Write("Kullanıcı Adınız: ");
            string? username = _tool.ReadLine();

            _tool.Write("Server ID: ");
            Guid serverId = Guid.Parse(_tool.ReadLine()!);

            await chatHub.JoinServer(username!, serverId);

            while (true)
            {
                string message = _tool.ReadLine()!;

                if (message.Equals("/exit", StringComparison.CurrentCultureIgnoreCase))
                    break;

                _tool.ClearLine(1);

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