using MessangerBack.Models;
using MessangerBack.Repositories;
using MessangerBack.Exceptions;


namespace MessangerBack.Services;

public class ChatService : IChatService
{
    IChatRepository _repository;
    IMessageService _messageService;

    public ChatService(IChatRepository repository, IMessageService messageService)
    {
        _repository = repository;
        _messageService = messageService;
    }

    public async Task CreateChat(Guid adminId, string chatName)
    {
        Guid[] users = { adminId };

        ChatModel chat = new()
        {
            Id = Guid.NewGuid(),
            AdminId = adminId, 
            Users = users,
            ChatName = chatName,
        };

        await _repository.CreateChat(chat);
        await _messageService.CreateMessage(adminId, chat.Id, "Создал чат");
    }

    public async Task AddToChat(Guid userId, string rawChatId)
    {
        ChatModel chat;
        try
        {
            Guid chatId = Guid.Parse(rawChatId);
            chat = await _repository.GetChatById(chatId);
        }
        catch (FormatException)
        {
            throw new WrongUserInputException("Id чата введён не верно.");
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException("Чат не найден.");
        }
        
        Guid[] chatMembers = chat.Users;

        if (chatMembers.Contains(userId))
        {
            throw new WrongUserInputException("Пользователь состоит в чате.");
        }

        chatMembers = chatMembers.Concat(new Guid[] { userId }).ToArray();

        chat.Users = chatMembers;
        await _repository.UpdateChat(chat);
    }
}
