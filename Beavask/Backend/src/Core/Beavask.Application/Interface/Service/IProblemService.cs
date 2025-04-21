using Beavask.Application.Common;
using Beavask.Application.DTOs.Problem;

namespace Beavask.Application.Interface.Service;

public interface IProblemService
{
    Task<Response<ProblemDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<ProblemDto>>> GetAllAsync();
    Task<Response<ProblemDto>> CreateAsync(ProblemCreateDto problemCreateDto);
    Task<Response<ProblemDto>> UpdateAsync(int id, ProblemUpdateDto problemUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
}
