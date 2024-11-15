using Chat.MenuOptions;

namespace Chat.Windows;

public class MainMenu(ITool tool, IEnumerable<IMainMenuOptions> mainMenuOptions) : Window
{
    readonly ITool _tool = tool;
    readonly List<IMainMenuOptions> _options = mainMenuOptions.ToList();
    int _currentIndex = 0;

    public override string Name => nameof(MainMenu);

    public override Task Open()
    {
        Console.CursorVisible = false;

        for (int i = 0; i < _options.Count; i++)
        {
            if (i == _currentIndex)
                _tool.WriteLine($"> {_options[i].Name}");
            else
                _tool.WriteLine($"  {_options[i].Name}");
        }

        var key = _tool.ReadKey().Key;

        if(key == ConsoleKey.DownArrow) _currentIndex = (_currentIndex + 1) % _options.Count;
        else if(key == ConsoleKey.UpArrow) _currentIndex = (_currentIndex - 1 + _options.Count) % _options.Count;
        else if(key == ConsoleKey.Enter) _options[_currentIndex].Execute();

        return Task.CompletedTask;
    }
}
