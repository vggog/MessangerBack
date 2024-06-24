namespace MessangerBack.Services;

public interface IMessageService
{
    public Task CreateMessage(Guid senderId, Guid chatId, string Text);
} 
