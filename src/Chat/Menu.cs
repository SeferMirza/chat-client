namespace Chat;

public class Menu(List<Option> options) : IDraw, IAction
{
    public async Task Action(string actionName)
    {
        Option option = options
            .First(option => option.Name == actionName) ?? throw new Exception("Option not registered");
        await option.ExecuteAsync();
    }

    public string Draw()
    {
        var result = string.Empty;

        int index = 1;
        foreach(var option in options)
        {
            result += $"{index}: {option.Name}" + Environment.NewLine;
            index++;
        }

        return result;
    }
}

public interface IAction
{
    Task Action(string actionName);
}

public interface IDraw
{
    string Draw();
}