namespace MessangerBack.Services;
using MessangerBack.Responces;


public interface IMessageService
{
    public Task CreateMessage(Guid senderId, Guid chatId, string Text);

    public Task<List<MessageResponce>> GetChatMessages(Guid chatId, Guid userId);
} 
