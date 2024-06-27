using MessangerBack.Models;

namespace MessangerBack.Repositories;

public interface IUsersRepository
{
    public Task<UserModel?> GetUserByEmail(string email); 
    public Task ChangePassword(UserModel user);
}
