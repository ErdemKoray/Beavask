using Beavask.Application.Common;
using Beavask.Application.DTOs.Message;

namespace Beavask.Application.Interface.Service;

public interface IMessageService
{
    Task<Response<MessageDto>> CreateAsync(MessageCreateDto messageCreateDto);
    Task<Response<IEnumerable<MessageDto>>> GetMessagesBySenderIdAsync(int senderId);
    Task<Response<IEnumerable<MessageDto>>> GetMessagesByReceiverIdAsync(int receiverId);
}

