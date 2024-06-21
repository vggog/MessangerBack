using MessangerBack.Schemas;


namespace MessangerBack.Services;


public interface IAuthService
{
    public Task RegisterUser(RegisterUserSchema createUserData);
}
