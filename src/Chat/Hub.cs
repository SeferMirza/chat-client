using Microsoft.AspNetCore.SignalR.Client;

namespace Chat;

public sealed class Hub
{
    readonly HubConnection _connection;
    string? clientId = string.Empty;

    public Hub()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5181/chatHub")
            .Build();
    }

    public async Task Connect()
    {
        await _connection.StartAsync();

        Console.WriteLine("SignalR bağlantısı kuruldu.");
    }

    public void Subscribe()
    {
        _connection.On<Message>("ReceiveMessage", (message) =>
        {
            Console.Write($"[{message.SentAt}] ");
            if(message.SenderId == clientId)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"You: ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{message.SenderId}: ");
            }

            Console.ResetColor();

            Console.Write($"{message.Content}");
            Console.WriteLine();
        });
    }

    public async Task SendMessage(string message, Guid roomId)
    {
        await _connection.InvokeAsync("SendMessage", roomId, message);
    }

    public async Task JoinRoom(Guid serverId, Guid roomId)
    {
        clientId = await _connection.InvokeAsync<string>("JoinRoom", serverId, roomId);
        Console.WriteLine($"{roomId} odasına katıldınız.");
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}