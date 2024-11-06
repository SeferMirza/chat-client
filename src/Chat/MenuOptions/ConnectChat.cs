using Microsoft.Extensions.DependencyInjection;

namespace Chat.MenuOptions;

public class ChatOption(IServiceProvider provider) : IMainMenuOptions
{
    Lazy<WindowStack> _windowStack = new(() => provider.GetRequiredService<WindowStack>());
    public string Name { get; } = "Chat";
    public void ExecuteAsync()
    {
        _windowStack.Value.Navigate(Name);
    }
}
