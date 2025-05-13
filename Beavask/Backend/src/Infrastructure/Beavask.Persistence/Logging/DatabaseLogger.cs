using Beavask.Application.Interface.Logging;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;

namespace Beavask.Persistence.Logging
{
    public class DatabaseLogger(IUnitOfWork unitOfWork) : ILogger
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async System.Threading.Tasks.Task LogInformation(string message, string? context = null, int? userId = null, int? companyId = null)
        {
            await TryLogAsync("Information", message, context, userId, companyId);
        }

        public async System.Threading.Tasks.Task LogError(string message, Exception? exception = null, string? context = null, int? userId = null, int? companyId = null)
        {
            var fullMessage = exception == null ? message : $"{message} | Exception: {exception.Message}";
            await TryLogAsync("Error", fullMessage, context, userId, companyId);
        }

        public async System.Threading.Tasks.Task LogWarning(string message, string? context = null, int? userId = null, int? companyId = null)
        {
            await TryLogAsync("Warning", message, context, userId, companyId);
        }

        public async System.Threading.Tasks.Task LogDebug(string message, string? context = null, int? userId = null, int? companyId = null)
        {
            await TryLogAsync("Debug", message, context, userId, companyId);
        }

        public async System.Threading.Tasks.Task LogTrace(string message, string? context = null, int? userId = null, int? companyId = null)
        {
            await TryLogAsync("Trace", message, context, userId, companyId);
        }

        private async System.Threading.Tasks.Task TryLogAsync(string activityType, string message, string? context, int? userId, int? companyId)
        {
            try
            {
                if (userId == null && companyId == null)
                    return; // Hiçbiri yoksa loglama yapılmaz

                User? user = null;
                Company? company = null;

                if (userId != null)
                    user = await _unitOfWork.UserRepository.GetByIdAsync(userId.Value);

                if (companyId != null)
                    company = await _unitOfWork.CompanyRepository.GetByIdAsync(companyId.Value);

                var log = new Log
                {
                    ActivityType = activityType,
                    Description = string.IsNullOrWhiteSpace(context)
                        ? message
                        : $"{message} | Context: {context}",
                    UserId = userId ?? null,
                    User = user,
                    CompanyId = companyId ?? null,
                    Company = company,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.LogRepository.AddAsync(log);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
