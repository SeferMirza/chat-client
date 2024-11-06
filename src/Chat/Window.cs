namespace Chat;

public abstract class Window
{
    public abstract string Name { get; }
    public abstract Task Open();
}
