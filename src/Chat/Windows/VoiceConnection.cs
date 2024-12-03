using Chat.Consoles;
using Chat.Hubs;

namespace Chat.Windows;

public class VoiceConnection(ServerService _server, VoiceHub voiceHub, ITool _tool, IRouter _router) : Window
{
    public override string Name => nameof(VoiceConnection);

    public async override Task Open()
    {
        _tool.Write("Username: ");
        var username = _tool.ReadLine();

        await voiceHub.Connect(_server.CurrentServer.ServerId, username);

        var cancellationTokenSource = new CancellationTokenSource();

        var updateTask = Task.Run(async () =>
        {
            List<User>? previousList = null;

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                var currentList = _server.CurrentServer.ConnectedUsers;

                if (!ListsAreEqual(previousList, currentList))
                {
                    _tool.ClearFull();
                    _tool.WriteLine("To exit press 'Escape' key!");
                    _tool.WriteLine(Environment.NewLine);
                    _tool.WriteLine("Connected Users");

                    foreach (var user in currentList)
                    {
                        _tool.WriteLine($"- {user.Name}");
                    }

                    previousList = [.. currentList];
                }

                await Task.Delay(1000);
            }
        });

        var keyListenerTask = Task.Run(() =>
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true); // TODO
                    if (key.Key == ConsoleKey.Escape)
                    {
                        cancellationTokenSource.Cancel();
                    }
                }

                Thread.Sleep(100);
            }
        });

        await Task.WhenAny(updateTask, keyListenerTask);

        cancellationTokenSource.Cancel();
        await voiceHub.DisposeAsync();
        _router.NavigateBack();
    }

    private bool ListsAreEqual(List<User> list1, List<User> list2)
    {
        if (list1 == null || list2 == null)
            return false;

        if (list1.Count != list2.Count)
            return false;

        return list1.SequenceEqual(list2);
    }
}
