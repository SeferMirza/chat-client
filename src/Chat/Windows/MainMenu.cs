using Chat.MenuOptions;

namespace Chat.Windows;

public class MainMenu(ITool tool, IEnumerable<IMainMenuOptions> mainMenuOptions) : Window
{
    readonly ITool _tool = tool;
    readonly List<IMainMenuOptions> _options = mainMenuOptions.ToList();
    int _currentIndex = 0;

    public override string Name => "MainMenu";

    public override async Task Open()
    {
        string result = string.Empty;
        Console.CursorVisible = false;

        for (int i = 0; i < _options.Count; i++)
        {
            if (i == _currentIndex)
                result += $"> {_options[i].Name}\n";
            else
                result += $"  {_options[i].Name}\n";
        }

        _tool.Write(result);

        var key = _tool.ReadKey().Key;

        if(key == ConsoleKey.DownArrow) _currentIndex = (_currentIndex + 1) % _options.Count;
        else if(key == ConsoleKey.UpArrow) _currentIndex = (_currentIndex - 1 + _options.Count) % _options.Count;
        else if(key == ConsoleKey.Enter) _options[_currentIndex].ExecuteAsync();
    }
}
