using MessangerBack.Models;

namespace MessangerBack.Repositories;

public interface IAuthRepository
{
    public Task Register(UserModel user);

    public Task<UserModel?> GetUserByUsername(string username);
}
