using Beavask.Application.Common;
using Beavask.Application.DTOs.Message;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Base;
using Beavask.Application.Interface;
using AutoMapper;

namespace Beavask.Application.Service;

public class MessageService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IMessageService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    public async Task<Response<MessageDto>> CreateAsync(MessageCreateDto messageCreateDto)
    {
        try
        {
            var senderId = _currentUserService.UserId.Value;
            var sender = await _unitOfWork.UserRepository.GetByIdAsync(senderId);
            var receiver = await _unitOfWork.UserRepository.GetByIdAsync(messageCreateDto.ReceiverId);
            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Content = messageCreateDto.Content,
                CreatedAt = DateTime.UtcNow,
            };
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

    public async Task<Response<IEnumerable<MessageDto>>> GetMessagesByFriendIdIdAsync(int friendId)
    {
        try
        {
            var currentUserId = _currentUserService.UserId.Value;
            var messages = await _unitOfWork.MessageRepository.GetMessagesByFriendIdAsync(currentUserId, friendId);
            var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);
            return Response<IEnumerable<MessageDto>>.Success(messageDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<MessageDto>>.Fail(ex.Message);
        }
    }
}

