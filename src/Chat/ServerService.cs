using Chat.Exceptions;
using System.Net.Http.Json;
using System.Text.Json;

namespace Chat;

public class ServerService(HttpClient _client)
{
    public string BaseUrl = "https://chat-server-production-fd56.up.railway.app";
    public string ServerApiPath => Path.Join(BaseUrl, "/server");
    public Server? CurrentServer { get; private set; }

    public async Task<List<Server>> GetServers()
    {
        var url = Path.Join(ServerApiPath, "/servers");
        List<Server>? result = await GetAsync<List<Server>>(url);

        return result!;
    }

    public async Task<Server> GetServer(Guid serverId)
    {
        var query = $"?id={serverId}";
        var url = Path.Join(ServerApiPath, "/server-detail", query);
        ServerDetail? result = await GetAsync<ServerDetail>(url);

        return new(result!.ServerId, result.ServerName, result.ServerType, result.Public);
    }

    public async Task<ServerDetail> GetServerDetail(Guid serverId)
    {
        var query = $"?id={serverId}";
        var url = Path.Join(ServerApiPath, "/server-detail", query);

        ServerDetail? result = await GetAsync<ServerDetail>(url);

        return result!;
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

    public void JoinServer(Server server)
    {
        CurrentServer = server;
    }
}
