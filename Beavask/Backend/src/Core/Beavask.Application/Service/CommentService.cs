using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Comment;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Logging;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Service;

public class CommentService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ILogger logger) : ICommentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    public async Task<Response<CommentDto>> CreateAsync(CommentCreateDto commentCreateDto)
    {
        try
        {
            var comment = _mapper.Map<Comment>(commentCreateDto);
            comment.UserId = _currentUserService.UserId ?? throw new Exception("User not found");
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(comment.TaskId);
            if (task == null)
            {
                return Response<CommentDto>.Fail("Task not found.");
            }
            if (task.AssignedUserId != _currentUserService.UserId)
            {
                return Response<CommentDto>.Fail("You are not authorized to add a comment to this task.");
            }
            await _unitOfWork.CommentRepository.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            await _logger.LogInformation($"Comment created successfully. CommentId: {comment.Id}, TaskId: {comment.TaskId}");
            return Response<CommentDto>.Success(mapper.Map<CommentDto>(comment));
        }
        catch (Exception ex)
        {
            await _logger.LogError(ex.Message);
            return Response<CommentDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return Response<bool>.Fail("Yorum bulunamadı.");
            }

            await _unitOfWork.CommentRepository.DeleteAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            await _logger.LogInformation($"Comment deleted successfully. CommentId: {id}");
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _logger.LogError($"Yorum silinirken hata: {ex.Message}");
            return Response<bool>.Fail(ex.Message);
        }
    }


    public async Task<Response<IEnumerable<CommentDto>>> GetAllCommentsByUserIdAsync(int userId)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Response<IEnumerable<CommentDto>>.Fail("User not found.");
            }
            var comments = await _unitOfWork.CommentRepository.GetAllByUserIdAsync(userId);
            var dtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Response<IEnumerable<CommentDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            await _logger.LogError($"Error while getting comments by user id: {ex.Message}");
            return Response<IEnumerable<CommentDto>>.Fail(ex.Message);
        }
    }


    public async Task<Response<CommentDto>> GetByIdAsync(int id)
    {
        try
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return Response<CommentDto>.Fail("Comment not found.");
            }

            var dto = _mapper.Map<CommentDto>(comment);
            return Response<CommentDto>.Success(dto);
        }
        catch (Exception ex)
        {
            await _logger.LogError($"Error while getting comment details: {ex.Message}");
            return Response<CommentDto>.Fail(ex.Message);
        }
    }


    public async Task<Response<CommentDto>> UpdateAsync(int id, CommentDto commentDto)
    {
        try
        {
            var existingComment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            if (existingComment == null)
            {
                return Response<CommentDto>.Fail("Comment not found.");
            }

            existingComment.Content = commentDto.Content;
            existingComment.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CommentRepository.UpdateAsync(existingComment);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<CommentDto>(existingComment);
            return Response<CommentDto>.Success(dto);
        }
        catch (Exception ex)
        {
            await _logger.LogError($"Error while updating comment: {ex.Message}");
            return Response<CommentDto>.Fail(ex.Message);
        }
    }
    public async Task<Response<IEnumerable<CommentDto>>> GetAllCommentsByTaskIdAsync(int taskId)
    {
        try
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                await _logger.LogWarning($"Task not found for task id: {taskId}");
                return Response<IEnumerable<CommentDto>>.Fail("Task not found.");
            }
            var comments = await _unitOfWork.CommentRepository.GetAllByTaskIdAsync(taskId);
            if (!comments.Any())
            {
                await _logger.LogWarning($"No comments found for task id: {taskId}");
                return Response<IEnumerable<CommentDto>>.Fail("No comments found for this task.");
            }
            await _logger.LogInformation($"Comments fetched successfully for task id: {taskId}");
            var dtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Response<IEnumerable<CommentDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            await _logger.LogError($"Error while getting comments by task id: {ex.Message}");
            return Response<IEnumerable<CommentDto>>.Fail(ex.Message);
        }
    }
}
