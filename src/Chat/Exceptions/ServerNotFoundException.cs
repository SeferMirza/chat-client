namespace Chat.Exceptions;

public class ServerConnectionException : HandledException
{
    public ServerConnectionException()
        : base("Server Connection Fail", "Server connection failed!", ExceptionLevel.Error) { }
}
