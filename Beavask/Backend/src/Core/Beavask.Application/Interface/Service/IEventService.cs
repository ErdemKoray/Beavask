using Beavask.Application.Common;
using Beavask.Application.DTOs.Event;

namespace Beavask.Application.Interface.Service;

public interface IEventService
{
    Task<Response<EventDto>> GetEventByIdAsync(int id);
    Task<Response<IEnumerable<EventDto>>> GetAllEventsAsync();
    Task<Response<EventDto>> CreateEventAsync(EventCreateDto eventDto);
    Task<Response<EventDto>> UpdateEventAsync(int id, EventUpdateDto eventDto);
    Task<Response<bool>> DeleteEventAsync(int id);
}
