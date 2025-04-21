using Beavask.Application.Common;
using Beavask.Application.DTOs.LogDtos;

namespace Beavask.Application.Interface.Service;

public interface ILogService
{
    Task<Response<LogDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<LogDto>>> GetAllAsync();
    Task<Response<LogDto>> CreateAsync(LogCreateDto logCreateDto);
}
