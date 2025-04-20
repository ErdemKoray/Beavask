namespace Beavask.Application.Common;

public class Response<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public Response(bool success, string message, T? data = default)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public static Response<T> SuccessResponse(T data, string message = "Operation successful")
    {
        return new Response<T>(true, message, data);
    }

    public static Response<T> ErrorResponse(string message, T? data = default)
    {
        return new Response<T>(false, message, data);
    }
}
