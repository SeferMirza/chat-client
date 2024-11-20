namespace Chat.Exceptions;

public interface IExceptionHandler
{
    void HandleException(Exception exception);
}
