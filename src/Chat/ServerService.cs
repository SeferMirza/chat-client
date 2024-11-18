using System.Net.Http.Json;

namespace Chat;

public class ServerService(HttpClient _client)
{
    const string ServerUrl = "http://localhost:5181/chat";

    public record Server(Guid ServerId, string ServerName);
    public record ServerDetail(Guid ServerId, string ServerName, int Capacity, List<string> ConnectedUsers);
    public async Task<Server> GetServer(Guid serverId)
    {
        var query = $"?id={serverId}";
        var url = Path.Join(ServerUrl, "/server-detail", query);
        HttpResponseMessage response = await _client.GetAsync(url);
        ServerDetail result = await response.Content.ReadFromJsonAsync<ServerDetail>() ?? throw new Exception("No service found");

        return new(result.ServerId, result.ServerName);
    }

    public async Task<ServerDetail> GetServerDetail(Guid serverId)
    {
        var query = $"?id={serverId}";
        var url = Path.Join(ServerUrl, "/server-detail", query);

        HttpResponseMessage response = await _client.GetAsync(url);
        ServerDetail result = await response.Content.ReadFromJsonAsync<ServerDetail>() ?? throw new Exception("No service found");

        return result;
    }

    public async Task<bool> CheckUsername(string username, Guid serverId)
    {
        var query = $"?serverId={serverId}&username={username}";
        var url = Path.Join(ServerUrl, "/username-is-valid", query);

        HttpResponseMessage response = await _client.GetAsync(url);
        var result = await response.Content.ReadAsStringAsync();

        return Convert.ToBoolean(result);
    }
}