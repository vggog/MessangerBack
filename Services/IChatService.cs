using MessangerBack.Models;
using MessangerBack.Responces;

namespace MessangerBack.Services;

public interface IChatService
{
    public Task<ChatInfoResponce> CreateChat(Guid adminId, string chatName);

    public Task AddToChat(Guid userId, Guid chatId);

    public Task<List<AllChatsResponce>> GetAllUserChats(Guid userId);

    public Task<ChatInfoResponce> GetChatInfo(Guid chatId);
}
