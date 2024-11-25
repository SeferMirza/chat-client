using Chat.Windows;

namespace Chat.MenuOptions;

public class VoiceOption(IRouter _router) : IMainMenuOptions
{
    public string Name { get; } = "Voice";
    public void Execute()
    {
        _router.Navigate(nameof(VoiceConnection));
    }
}
