using MessangerBack.Models;

namespace MessangerBack.Repositories;

public interface IChangePasswordRepository
{
    public Task<UserModel?> GetUserByEmail(string email); 
    public Task ChangePassword(UserModel user, string newPassword);
}
