using Microsoft.Extensions.DependencyInjection;

namespace Chat.MenuOptions;

public class ServersOption(IServiceProvider provider) : IMainMenuOptions
{
    Lazy<WindowStack> _windowStack = new(() => provider.GetRequiredService<WindowStack>());
    public string Name { get; } = "Servers";

    public void ExecuteAsync()
    {
        _windowStack.Value.Navigate(Name);
    }
}
