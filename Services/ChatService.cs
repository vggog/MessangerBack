using MessangerBack.Models;
using MessangerBack.Repositories;

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
}
