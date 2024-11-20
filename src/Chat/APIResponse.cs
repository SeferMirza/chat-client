namespace Chat;

public class APIResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}
