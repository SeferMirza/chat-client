namespace Chat.Exceptions;

public class ServerError(string title, string message)
    : HandledException(title, message, ExceptionLevel.Error);
