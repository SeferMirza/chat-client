using Chat.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Chat;

public class WindowManager : IWindowManager
{
    readonly Stack<string> _stack = new();
    readonly IServiceProvider _serviceProvider;
    private readonly IRouter _router;
    Window _current;

    public WindowManager(IServiceProvider serviceProvider, IRouter router)
    {
        _serviceProvider = serviceProvider;
        _router = router;
        _current = _serviceProvider.GetRequiredKeyedService<Window>(_router.Path ?? nameof(MainMenu));
    }

    public async Task OpenCurrentWindow()
    {
        _current = _serviceProvider.GetRequiredKeyedService<Window>(_router.Path ?? nameof(MainMenu));
        await _current.Open();
    }
}

public interface IWindowManager
{
    Task OpenCurrentWindow();
}
