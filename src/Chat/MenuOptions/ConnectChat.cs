using Microsoft.Extensions.DependencyInjection;

namespace Chat.MenuOptions;

public class ChatOption(IServiceProvider provider) : IMainMenuOptions
{
    Lazy<WindowStack> _windowStack = new(() => provider.GetRequiredService<WindowStack>());
    public string Name { get; } = "Chat";
    public void Execute()
    {
        _windowStack.Value.Navigate(Name);
    }
}
