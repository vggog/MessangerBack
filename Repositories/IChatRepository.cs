using MessangerBack.Models;

namespace MessangerBack.Repositories;

public interface IChatRepository
{
    public Task<List<ChatModel>> AllChatsFilterByChatName(string chatName);
    public Task CreateChat(ChatModel chat);
    public Task<ChatModel> GetChatById(Guid chatId);
    public Task UpdateChat(ChatModel chat);

    public Task<List<ChatModel>> GetAllUserChats(Guid userId);
}
