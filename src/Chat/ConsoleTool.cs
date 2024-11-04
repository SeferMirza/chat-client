namespace Chat;

public class ConsoleTool : ITool
{
    public void Clear()
    {
        int cursorTop = Console.CursorTop - 1;

        Console.SetCursorPosition(0, cursorTop);
        Console.Write(new string(' ', Console.WindowWidth));

        Console.SetCursorPosition(0, cursorTop);
    }

    public string Read()
    {
        return Console.ReadLine() ?? string.Empty;
    }

    public void Write(string text)
    {
        Console.Write(text);
    }
}

public interface ITool
{
    string Read();
    void Write(string text);
    void Clear();
}