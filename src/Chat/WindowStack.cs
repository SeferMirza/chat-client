namespace Chat;

public class WindowStack
{
    Window _current;
    readonly Dictionary<string, Window> _stack = [];

    public WindowStack(IEnumerable<Window> windows)
    {
        windows.ToList().ForEach(window => _stack.Add(window.Name, window));
        _current = Stack.FirstOrDefault().Value;
    }

    public Window Current => _current;
    public Dictionary<string, Window> Stack => _stack;

    public void Navigate(string windowName)
    {
        _current = _stack[windowName];
    }
}
