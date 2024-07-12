using MessangerBack.DataBase;
using MessangerBack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MessangerBack.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly DataBaseContext _context;

    public UsersRepository(DataBaseContext context)
        => _context = context;

    public async Task<UserModel?> GetUserByEmail(string email)
    {
        //return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return new();
    }

    public async Task ChangePassword(UserModel user)
    {
        // _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IdentityUser<System.Guid>> GetUserById(Guid userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}
