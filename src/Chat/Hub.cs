using Microsoft.AspNetCore.SignalR.Client;

namespace Chat;

public sealed class Hub(ITool tool)
{
    readonly HubConnection _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5181/chatHub")
            .Build();
    readonly ITool _tool = tool;
    string? userId = string.Empty;
    string? userName = string.Empty;

    public async Task Connect()
    {
        await _connection.StartAsync();

        Console.WriteLine("Server connected.");
    }

    public void Subscribe()
    {
        _connection.On<Message>("ReceiveMessage", (message) =>
        {
            _tool.Write($"[{message.SentAt}] ");
            if(message.Sender == userName)
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
        });
    }

    public async Task SendMessage(Guid serverId, string message)
    {
        await _connection.InvokeAsync("SendMessage", serverId, message);
    }

    public async Task<List<Message>> JoinServer(string name, Guid serverId)
    {
        userName = name;
        var oldMessages = await _connection.InvokeAsync<List<Message>>("JoinServer", name, serverId);
        _tool.WriteLine($"You joined server '{serverId}' as '{name}'.");

        return oldMessages;
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}
