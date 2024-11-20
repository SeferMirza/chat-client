namespace Chat.Exceptions;

public class ConnectionException()
    : HandledException(
        title: "No Connection",
        message: "Connection is not established!",
        ExceptionLevel.Error
    );
