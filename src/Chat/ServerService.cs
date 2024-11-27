using Chat.Exceptions;
using System.Net.Http.Json;
using System.Text.Json;

namespace Chat;

public class ServerService(HttpClient _client)
{
    public string ServerUrl = "http://localhost:5181";
    public const string ChatServerPath = "/chat";
    public const string VoiceServerPath = "/voice";

    public record Server(Guid ServerId, string ServerName);
    public record ServerDetail(Guid ServerId, string ServerName, int Capacity, List<string> ConnectedUsers);

    public async Task<List<Server>> GetServers()
    {
        var url = Path.Join(ServerUrl, ChatServerPath, "/servers");
        List<Server>? result = await GetAsync<List<Server>>(url);

        return result!;
    }

    public async Task<Server> GetServer(Guid serverId)
    {
        var query = $"?id={serverId}";
        var url = Path.Join(ChatServerPath, "/server-detail", query);
        ServerDetail? result = await GetAsync<ServerDetail>(url);

        return new(result!.ServerId, result.ServerName);
    }

    public async Task<ServerDetail> GetServerDetail(Guid serverId)
    {
        var query = $"?id={serverId}";
        var url = Path.Join(ChatServerPath, "/server-detail", query);

        ServerDetail? result = await GetAsync<ServerDetail>(url);

        return result!;
    }

    public async Task<bool> CheckUsername(string username, Guid serverId)
    {
        if (string.IsNullOrEmpty(username)) throw new UsernameCannotBeNullException();

        var query = $"?serverId={serverId}&username={username}";
        var url = Path.Join(ChatServerPath, "/username-is-valid", query);

        var result = await GetAsync<bool>(url);

        return result;
    }

    async Task<T?> GetAsync<T>(string url)
    {
        HttpResponseMessage response;
        APIResponse<object> result;
        try
        {
            response = await _client.GetAsync(url);
            result = await response.Content.ReadFromJsonAsync<APIResponse<object>>() ?? throw new ArgumentNullException();
        }
        catch (HttpRequestException)
        {
            throw new ServerConnectionException();
        }
        catch (TaskCanceledException)
        {
            throw new ServerConnectionException();
        }

        if (!result.Success)
        {
            throw new ServerError(result.Error!, result.Message!);
        }

        return ((JsonElement)result.Data!).Deserialize<T>(options: new()
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<List<Server>> GetVoiceServers()
    {
        var url = Path.Join(ServerUrl, VoiceServerPath, "/servers");
        List<Server>? result = await GetAsync<List<Server>>(url);

        return result!;
    }
}
