using MessangerBack.Repositories;
using MessangerBack.Responces;
using MessangerBack.Exceptions;
using MessangerBack.Models;


namespace MessangerBack.Services;

public class MessageService : IMessageService
{
    IMessageRepository _repository;
    IChatRepository _chatRepository;
    IUsersRepository _userRepository;

    public MessageService(IMessageRepository repository, IChatRepository chatRepository, IUsersRepository userRepository)
    {
        _repository = repository;
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    public async Task<MessageResponce> CreateMessage(Guid senderId, Guid chatId, string text)
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
        var user = await _userRepository.GetUserById(senderId);

        return new()
        {
            Id = message.Id,
            SenderId = message.SenderId,
            Sender = new()
            {
                Id = user.Id,
                UserName = user.UserName
            },
            Text = message.Text,
            MessageSentTime = message.MessageSentTime
        };
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
