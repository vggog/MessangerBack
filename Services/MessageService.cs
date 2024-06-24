using MessangerBack.Repositories;
using MessangerBack.Models;

namespace MessangerBack.Services;

public class MessageService : IMessageService
{
    IMessageRepository _repository;
    IChatRepository _chatRepository;

    public MessageService(IMessageRepository repository, IChatRepository chatRepository)
    {
        _repository = repository;
        _chatRepository = chatRepository;
    }

    public async Task CreateMessage(Guid senderId, Guid chatId, string text)
    {
        MessageModel message = new()
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            SenderId = senderId,
            Text = text,
            MessageSentTime = DateTime.UtcNow
        };

        await _repository.CreateMessage(message);

        ChatModel chat = await _chatRepository.GetChatById(chatId);
        chat.LastMessageId = message.Id;

        await _chatRepository.UpdateChat(chat);
    }
} 
