namespace Chat;

public class ConsoleTool : ITool
{
    public void ClearFull()
    {
        Console.Clear();
    }

    public void ClearLine(int lineCount)
    {
        int cursorTop = Console.CursorTop - lineCount;

        for (int i = 0; i < lineCount; i++)
        {
            Console.SetCursorPosition(0, cursorTop + i);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        Console.SetCursorPosition(0, cursorTop);
    }

    public ConsoleKeyInfo ReadKey() => Console.ReadKey();

    public string ReadLine() => Console.ReadLine() ?? string.Empty;

    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }

    public void Write(string text)
    {
        Console.Write(text);
    }
}

public interface ITool
{
    string ReadLine();
    ConsoleKeyInfo ReadKey();
    void Write(string text);
    void WriteLine(string text);
    void ClearLine(int lineCount);
    void ClearFull();
}