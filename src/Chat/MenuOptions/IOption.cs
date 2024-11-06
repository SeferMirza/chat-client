namespace Chat;

public interface IOption
{
    string Name { get; }
    void ExecuteAsync();
}
