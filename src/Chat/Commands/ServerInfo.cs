using Chat.Consoles;
using Chat.Exceptions;

namespace Chat.Commands;

public class ServerInfo(ITool _tool, ServerService _serverService) : ICommand
{
    public string Command => "/info";

    public void Execute()
    {
        ExecuteAsync().GetAwaiter();
    }

    public async Task ExecuteAsync()
    {
        try
        {
            Server result = await _serverService.GetServerDetail(_serverService.CurrentServer?.ServerId ?? throw new ServerConnectionException());

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
                    _tool.Write(user.Name);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    _tool.Write(new string(' ', maxWidth - 7 - user.Name.Length - 1) + "║" + Environment.NewLine);
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                _tool.WriteLine("╚" + new string('═', maxWidth - 2) + "╝");

                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }
}
