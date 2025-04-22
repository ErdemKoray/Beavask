using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.NotificationDtos;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Service;
public class NotificationService(IUnitOfWork unitOfWork, IMapper mapper) : INotificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<IEnumerable<NotificationDto>>> GetAllAsync()
    {
        try
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
            return Response<IEnumerable<NotificationDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<NotificationDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<NotificationDto>> CreateAsync(NotificationCreateDto dto)
    {
        try
        {
            var entity = _mapper.Map<Notification>(dto);
            await _unitOfWork.NotificationRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var createdDto = _mapper.Map<NotificationDto>(entity);
            return Response<NotificationDto>.Success(createdDto);
        }
        catch (Exception ex)
        {
            return Response<NotificationDto>.Fail(ex.Message);
        }
    }
}
