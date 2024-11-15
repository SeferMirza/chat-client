using Chat.Windows;

namespace Chat.MenuOptions;

public class ServersOption(IRouter _router) : IMainMenuOptions
{
    public string Name { get; } = "Servers";

    public void Execute()
    {
        _router.Navigate(nameof(Servers));
    }
}
