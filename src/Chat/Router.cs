namespace Chat;

public class Router : IRouter
{
    public string? Path { get; private set; }

    public void Navigate(string window)
    {
        Path = window;
    }
}

public interface IRouter
{
    string? Path { get; }
    void Navigate(string window);
}
