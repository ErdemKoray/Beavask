namespace Beavask.Application.Interface.Logging;
public interface ILogger
{
    Task LogInformation(string message, string? context = null, int? userId = null, int? companyId = null);
    Task LogError(string message, Exception? exception = null, string? context = null, int? userId = null, int? companyId = null);
    Task LogWarning(string message, string? context = null, int? userId = null, int? companyId = null);
    Task LogDebug(string message, string? context = null, int? userId = null, int? companyId = null);
    Task LogTrace(string message, string? context = null, int? userId = null, int? companyId = null);
}

