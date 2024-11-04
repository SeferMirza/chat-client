using System.Net.Http.Json;

namespace Chat;

public static class ServerApi
{
    const string ServerUrl = "http://localhost:5181/chat";
    static readonly HttpClient _httpClient = new();

    record ServerInfo(Guid ServerId, string ServerName);
    public static Func<Task> GetServers => async () => {
        try
        {
            var url = Path.Join(ServerUrl, "/servers");
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            List<ServerInfo> result = await response.Content.ReadFromJsonAsync<List<ServerInfo>>() ?? [];

            foreach(ServerInfo item in result)
            {
                Console.WriteLine($"{item.ServerName}: {item.ServerId}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    };
}
