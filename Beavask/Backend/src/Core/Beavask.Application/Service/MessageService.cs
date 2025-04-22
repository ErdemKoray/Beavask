using Beavask.Application.Common;
using Beavask.Application.DTOs.Message;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Base;
using Beavask.Application.Interface;
using AutoMapper;

namespace Beavask.Application.Service;

public class MessageService(IUnitOfWork unitOfWork, IMapper mapper) : IMessageService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<MessageDto>> CreateAsync(MessageCreateDto messageCreateDto)
    {
        try
        {
            var message = _mapper.Map<Message>(messageCreateDto);
            await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();
            
            var messageDto = _mapper.Map<MessageDto>(message);
            return Response<MessageDto>.Success(messageDto);
        }
        catch (Exception ex)
        {
            return Response<MessageDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<MessageDto>>> GetMessagesBySenderIdAsync(int senderId)
    {
        try
        {
            // Gönderilen ve alınan mesajları tek bir sorguda al
            var messages = await _unitOfWork.MessageRepository.GetAsync(query => 
                query.Where(m => m.SenderId == senderId || m.ReceiverId == senderId));

            // Mesajları MessageDto'ya dönüştür
            var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);

            return Response<IEnumerable<MessageDto>>.Success(messageDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<MessageDto>>.Fail(ex.Message);
        }
    }


    public async Task<Response<IEnumerable<MessageDto>>> GetMessagesByReceiverIdAsync(int receiverId)
    {
        try
        {
            var receivedMessages = await _unitOfWork.MessageRepository.GetAsync(query => 
                query.Where(m => m.ReceiverId == receiverId));

            var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(receivedMessages);

            return Response<IEnumerable<MessageDto>>.Success(messageDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<MessageDto>>.Fail(ex.Message);
        }
    }
}

