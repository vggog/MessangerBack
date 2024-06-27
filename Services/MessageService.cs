using MessangerBack.Repositories;
using MessangerBack.Models;
using MessangerBack.Responces;
using MessangerBack.Exceptions;


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

    public async Task<List<MessageResponce>> GetChatMessages(Guid chatId, Guid userId)
    {
        ChatModel chat;
        try
        {
            chat = await _chatRepository.GetChatById(chatId);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException("Чат не найден.");
        }

        if (!chat.Users.Contains(userId))
        {
            throw new NotFoundException("Чат не найден.");
        }
        
        var messages = await _repository.GetChatMessages(chatId);

        List<MessageResponce> responce = new();
        foreach (var message in messages)
        {
            responce.Add(
                new()
                {
                    Id = message.Id,
                    SenderId = message.SenderId,
                    Sender = new()
                    {
                        Id = message.Sender.Id,
                        UserName = message.Sender.UserName
                    },
                    Text = message.Text,
                    MessageSentTime = message.MessageSentTime
                }
            );
        }

        return responce;
    }
} 
