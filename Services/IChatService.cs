namespace MessangerBack.Services;

public interface IChatService
{
    public Task CreateChat(Guid adminId, string chatName);

    public Task AddToChat(Guid userId, Guid chatId);
}
