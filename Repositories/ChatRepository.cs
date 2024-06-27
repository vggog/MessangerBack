using Microsoft.EntityFrameworkCore;
using MessangerBack.DataBase;
using MessangerBack.Models;


namespace MessangerBack.Repositories;

public class ChatRepository : IChatRepository
{
    DataBaseContext _context;

    public ChatRepository(DataBaseContext context)
    {
        _context = context;
    }

    public async Task CreateChat(ChatModel chat)
    {
        await _context.Chats.AddAsync(chat);
        await _context.SaveChangesAsync();
    }

    public async Task<ChatModel> GetChatById(Guid chatId)
    {
        return await _context.Chats.FirstAsync(c => c.Id == chatId);
    }

    public async Task UpdateChat(ChatModel chat)
    {
        _context.Chats.Update(chat);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ChatModel>> GetAllUserChats(Guid userId)
    {
        return await _context.Chats.Where(c => c.Users.Contains(userId)).ToListAsync();
    }
}
