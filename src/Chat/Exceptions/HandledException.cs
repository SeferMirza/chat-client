namespace Chat.Exceptions;

public class HandledException(string title, string message, ExceptionLevel level)
    : Exception(message)
{
    public ExceptionLevel Level { get; } = level;
    public string Title { get; } = title;
}

public enum ExceptionLevel
{
    Warning,
    Error
}
