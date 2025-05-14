using Beavask.Application.Common;
using Beavask.Application.DTOs.Comment;

namespace Beavask.Application.Interface.Service;

public interface ICommentService
{
    Task<Response<CommentDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<CommentDto>>> GetAllCommentsByUserIdAsync(int userId);
    Task<Response<CommentDto>> CreateAsync(CommentCreateDto commentDto);
    Task<Response<CommentDto>> UpdateAsync(int id, CommentDto commentDto);
    Task<Response<bool>> DeleteAsync(int id);
}
