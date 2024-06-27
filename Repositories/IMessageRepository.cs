using MessangerBack.Models;

namespace MessangerBack.Repositories;

public interface IMessageRepository
{
    public Task CreateMessage(MessageModel message);

    public Task<List<MessageModel>> GetChatMessages(Guid chatId);
}
