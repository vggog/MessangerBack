using MessangerBack.Models;
using Microsoft.AspNetCore.Identity;


namespace MessangerBack.Repositories;

public interface IUsersRepository
{
    public Task<UserModel?> GetUserByEmail(string email); 
    public Task ChangePassword(UserModel user);
    public Task<IdentityUser<System.Guid>> GetUserById(Guid userId);
}
