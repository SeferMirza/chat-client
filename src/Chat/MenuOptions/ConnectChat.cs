using Chat.Windows;

namespace Chat.MenuOptions;

public class ChatOption(IRouter _router) : IMainMenuOptions
{
    public string Name { get; } = "Chat";
    public void Execute()
    {
        _router.Navigate(nameof(ChatConnection));
    }
}
