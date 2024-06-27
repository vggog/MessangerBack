using MessangerBack.DataBase;
using MessangerBack.Exceptions;
using MessangerBack.Models;
using MessangerBack.Schemas;
using MessangerBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace MessangerBack.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly DataBaseContext _context;

    public UsersRepository(DataBaseContext context)
        => _context = context;

    public async Task<UserModel?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task ChangePassword(UserModel user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
