using Microsoft.AspNetCore.SignalR.Client;

namespace Chat;

public sealed class Hub
{
    readonly HubConnection _connection;
    string? userId = string.Empty;
    string? userName = string.Empty;

    public Hub()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5181/chatHub")
            .Build();
    }

    public async Task Connect()
    {
        await _connection.StartAsync();

        Console.WriteLine("Server connected.");
    }

    public void Subscribe()
    {
        _connection.On<Message>("ReceiveMessage", (message) =>
        {
            Console.Write($"[{message.SentAt}] ");
            if(message.Sender == userName)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"You: ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{message.Sender}: ");
            }

            Console.ResetColor();

            Console.Write($"{message.Content}");
            Console.WriteLine();
        });
    }

    public async Task SendMessage(Guid serverId, string message)
    {
        await _connection.InvokeAsync("SendMessage", serverId, message);
    }

    public async Task JoinServer(string name, Guid serverId)
    {
        userName = name;
        userId = await _connection.InvokeAsync<string>("JoinServer", name, serverId);
        Console.WriteLine($"You joined server '{serverId}' as '{name}'.");
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}