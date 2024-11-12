namespace Chat;

public class ConsoleInfo : IConsoleInfo
{
    ConsoleColor _backgroundColor = Console.BackgroundColor;
    ConsoleColor _foregroundColor = Console.ForegroundColor;
    int _height = 0;

    public int Height => _height;

    public ConsoleColor BackgroundColor => _backgroundColor;

    public ConsoleColor ForegroundColor => _foregroundColor;

    public void ChangeColor(ConsoleColor? background = null, ConsoleColor? foreground = null)
    {
        _backgroundColor = background ?? Console.BackgroundColor;
        _foregroundColor = foreground ?? Console.ForegroundColor;
    }

    public void DecreaseHeight(int height)
    {
        _height -= height;
    }

    public void IncreaseHeight(int height)
    {
        _height += height;
    }
}

public interface IConsoleInfo
{
    int Height { get; }
    ConsoleColor BackgroundColor { get; }
    ConsoleColor ForegroundColor { get; }

    void IncreaseHeight(int height);
    void DecreaseHeight(int height);
    void ChangeColor(ConsoleColor? background = default, ConsoleColor? foreground = default);
}
