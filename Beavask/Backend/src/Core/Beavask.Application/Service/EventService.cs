using Beavask.Application.Common;
using Beavask.Application.DTOs.Event;
using Beavask.Application.Interface;
using AutoMapper;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Service;

public class EventService(IUnitOfWork unitOfWork, IMapper mapper) : IEventService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<EventDto>> CreateEventAsync(EventCreateDto eventDto)
    {
        try
        {
            var entity = _mapper.Map<Event>(eventDto);
            await _unitOfWork.EventRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            
            var dto = _mapper.Map<EventDto>(entity);
            return Response<EventDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<EventDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteEventAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.EventRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Response<bool>.NotFound();
            }

            await _unitOfWork.EventRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<EventDto>>> GetAllEventsAsync()
    {
        try
        {
            var entities = await _unitOfWork.EventRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<EventDto>>(entities);
            return Response<IEnumerable<EventDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<EventDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<EventDto>> GetEventByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.EventRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<EventDto>.NotFound();

            var dto = _mapper.Map<EventDto>(entity);
            return Response<EventDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<EventDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<EventDto>> UpdateEventAsync(int id, EventUpdateDto eventDto)
    {
        try
        {
            var existingEntity = await _unitOfWork.EventRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                return Response<EventDto>.NotFound();
            }

            _mapper.Map(eventDto, existingEntity);
            await _unitOfWork.EventRepository.UpdateAsync(existingEntity);
            await _unitOfWork.SaveChangesAsync();
            
            var dto = _mapper.Map<EventDto>(existingEntity);
            return Response<EventDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<EventDto>.Fail(ex.Message);
        }
    }
}
