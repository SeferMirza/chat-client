using Chat.Windows;

namespace Chat;

public class Router : IRouter
{
    string? prePath;
    public string? Path { get; private set; }

    public void Navigate(string window)
    {
        prePath = Path;
        Path = window;
    }

    public void NavigateRoot()
    {
        prePath = Path;
        Path = nameof(Servers);
    }

    public void NavigateBack()
    {
        (prePath, Path) = (Path, prePath);
    }
}

public interface IRouter
{
    string? Path { get; }
    void Navigate(string window);
    void NavigateRoot();
    void NavigateBack();
}
