using MessangerBack.DataBase;
using MessangerBack.Exceptions;
using MessangerBack.Models;
using MessangerBack.Schemas;
using MessangerBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace MessangerBack.Repositories;

public class ChangePasswordRepository : IChangePasswordRepository
{
    private readonly DataBaseContext _context;
    private readonly IPasswordUtils _passwordUtils;

    public ChangePasswordRepository(
        IPasswordUtils passwordUtils,
        DataBaseContext context)
    {
        _passwordUtils = passwordUtils;
        _context = context;
    }

    public async Task<UserModel?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task ChangePassword(UserModel user, string newPassword)
    {
        user.PasswordHash = _passwordUtils.GeneratePasswordHash(newPassword);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
