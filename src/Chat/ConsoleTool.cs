namespace Chat;

public class ConsoleTool(IConsoleInfo consoleInfo) : ITool
{
    public void ClearFull()
    {
        Console.Clear();
        consoleInfo.DecreaseHeight(consoleInfo.Height);
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
        consoleInfo.DecreaseHeight(lineCount);
    }

    public ConsoleKeyInfo ReadKey() => Console.ReadKey();

    public string ReadLine()
    {
        var input = Console.ReadLine() ?? string.Empty;
        consoleInfo.IncreaseHeight(1);

        return input;
    }

    public void WriteLine(string text)
    {
        Console.WriteLine(text);
        var newlinesCount = text.Split(Environment.NewLine).Length - 1;
        consoleInfo.IncreaseHeight(1 + newlinesCount);

    }

    public void Write(string text) => Console.Write(text);
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
