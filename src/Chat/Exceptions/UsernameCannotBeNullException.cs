namespace Chat.Exceptions;

public class UsernameCannotBeNullException()
    : HandledException(
        title: "Username Cannot Be Null Exception",
        message: "Username value cannot be null or empty!",
        ExceptionLevel.Warning
    );