
using System.Net.Http.Json;

namespace Chat.Commands;

public class ServerDetail(ITool _tool, HttpClient _client) : ICommand
{
    public string Command => "/info";
    const string ServerUrl = "http://localhost:5181/chat";

    public void Execute(params string[] args)
    {
        ExecuteAsync().GetAwaiter();
    }

    record ServerDetailDTO(Guid ServerId, string ServerName, int Capacity, List<string> ConnectedUsers);
    public async Task ExecuteAsync(params string[] args)
    {
        try
        {
            var filterParam = args.Where(arg => arg.StartsWith("--serverId")).FirstOrDefault() ?? throw new ArgumentNullException("ServerId must be given");
            var serverId = filterParam.Split(' ')[1];
            var query = $"?id={serverId}";
            var url = Path.Join(ServerUrl, "/server-detail", query);

            HttpResponseMessage response = await _client.GetAsync(url);
            ServerDetailDTO? result = await response.Content.ReadFromJsonAsync<ServerDetailDTO>() ?? null;
            if (result != null)
            {
                int maxWidth = result.ServerName.Length + 20 > 60 ? result.ServerName.Length + 20 : 60;

                Console.ForegroundColor = ConsoleColor.Magenta;
                _tool.WriteLine("╔" + new string('═', maxWidth - 2) + "╗");
                _tool.Write("║");
                Console.ForegroundColor = ConsoleColor.Green;
                _tool.Write(" Server Name: ");
                Console.ForegroundColor = ConsoleColor.White;
                _tool.Write(result.ServerName);
                Console.ForegroundColor = ConsoleColor.Magenta;
                _tool.Write(new string(' ', maxWidth - 15 - result.ServerName.Length - 1) + "║" + Environment.NewLine);

                _tool.Write("║");
                Console.ForegroundColor = ConsoleColor.Green;
                _tool.Write(" Server Id: ");
                Console.ForegroundColor = ConsoleColor.White;
                _tool.Write(result.ServerId.ToString());
                Console.ForegroundColor = ConsoleColor.Magenta;
                _tool.Write(new string(' ', maxWidth - 13 - result.ServerId.ToString().Length - 1) + "║" + Environment.NewLine);

                _tool.Write("║");
                Console.ForegroundColor = ConsoleColor.Green;
                _tool.Write(" Capacity: ");
                Console.ForegroundColor = ConsoleColor.White;
                _tool.Write(result.Capacity.ToString());
                Console.ForegroundColor = ConsoleColor.Magenta;
                _tool.Write(new string(' ', maxWidth - 12 - result.Capacity.ToString().Length - 1) + "║" + Environment.NewLine);

                _tool.Write("║");
                Console.ForegroundColor = ConsoleColor.Green;
                _tool.Write(" Connected Users:");
                Console.ForegroundColor = ConsoleColor.Magenta;
                _tool.Write(new string(' ', maxWidth - 18 - 1) + "║" + Environment.NewLine);

                foreach(var user in result.ConnectedUsers)
                {
                    _tool.Write("║");
                    Console.ForegroundColor = ConsoleColor.Green;
                    _tool.Write("    - ");
                    Console.ForegroundColor = ConsoleColor.White;
                    _tool.Write(user);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    _tool.Write(new string(' ', maxWidth - 7 - user.Length - 1) + "║" + Environment.NewLine);
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                _tool.WriteLine("╚" + new string('═', maxWidth - 2) + "╝");

                Console.ResetColor(); // TODO
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }
}