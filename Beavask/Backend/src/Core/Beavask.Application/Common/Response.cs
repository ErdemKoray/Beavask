namespace Beavask.Application.Common;

public class Response<T>
{
    public bool IsSuccess  { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public Response(bool success, string message, T? data = default)
    {
        IsSuccess = success;
        Message = message;
        Data = data;
    }

 public static Response<T> Success(T data, string message = "Operation successful")
        => new(true, message, data);

    public static Response<T> Fail(string message, List<string>? errors = null)
        => new(false, message) { Errors = errors };

    public static Response<T> NotFound(string message = "Not found")
        => new(false, message);
}
