namespace MessangerBack.Services;
using MessangerBack.Responces;
using MessangerBack.Models;


public interface IMessageService
{
    public Task<MessageResponce> CreateMessage(Guid senderId, Guid chatId, string Text);

    public Task<List<MessageResponce>> GetChatMessages(Guid chatId, Guid userId);
} 
