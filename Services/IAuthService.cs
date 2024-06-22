using MessangerBack.Schemas;


namespace MessangerBack.Services;


public interface IAuthService
{
    public Task RegisterUser(RegisterUserSchema createUserData);

    public Task<string> LoginUser(LoginUserSchema userData);
}
