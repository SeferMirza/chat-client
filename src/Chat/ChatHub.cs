using Chat.Exceptions;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chat;

public sealed class ChatHub(ITool tool)
{
    HubConnection? _connection;
    readonly ITool _tool = tool;
    string? _userName = string.Empty;
    Guid _serverId = Guid.Empty;

    private HubConnection CreateConnection()
    {
        return new HubConnectionBuilder()
            .WithUrl("http://192.168.1.113:5181/chatHub")
            .Build();
    }

    public async Task Connect()
    {
        _connection ??= CreateConnection();

        await _connection.StartAsync();

        _tool.WriteLine("Server connected.");
    }

    public void Subscribe()
    {
        if (_connection == null)
            throw new ConnectionException();

        _connection.On<Message>("ReceiveMessage", (message) =>
        {
            _tool.WriteChatMessage(message, _userName);
        });
    }

    public async Task SendMessage(string message)
    {
        if (_connection == null)
            throw new ConnectionException();

        await _connection.InvokeAsync("SendMessage", _serverId, message);
    }

    public async Task<List<Message>> JoinServer(string name, Guid serverId)
    {
        if (_connection == null)
            throw new ConnectionException();

        _userName = name;
        _serverId = serverId;
        var oldMessages = await _connection.InvokeAsync<List<Message>>("JoinServer", name, serverId);
        _tool.WriteLine($"You joined server '{serverId}' as '{name}'.");

        return oldMessages;
    }

    public async Task DisposeAsync()
    {
        if(_connection != null)
        {
            _connection.Remove("ReceiveMessage");
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }
    }
}
