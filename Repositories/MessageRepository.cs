using Microsoft.EntityFrameworkCore;
using MessangerBack.DataBase;
using MessangerBack.Models;


namespace MessangerBack.Repositories;

public class MessageRepository : IMessageRepository
{
    DataBaseContext _context;

    public MessageRepository(DataBaseContext context)
    {
        _context = context;
    }

    public async Task CreateMessage(MessageModel message)
    {
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
    }
}
