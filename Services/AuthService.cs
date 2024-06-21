using MessangerBack.Models;
using MessangerBack.Repositories;
using MessangerBack.Schemas;
using MessangerBack.Utils;


namespace MessangerBack.Services;

public class AuthService : IAuthService
{
    IPasswordUtils _passwordUtils;
    IAuthRepository _repository;

    public AuthService(IAuthRepository repository, IPasswordUtils passwordUtils)
    {
        _repository = repository;
        _passwordUtils = passwordUtils;
    }

    public async Task RegisterUser(RegisterUserSchema registerUserData)
    {
        string passwordHash = _passwordUtils.GeneratePasswordHash(registerUserData.Password);

        UserModel userModel = new() 
        {  
            Id = Guid.NewGuid(),
            UserName = registerUserData.UserName,
            PasswordHash = passwordHash,
            Email = registerUserData.Email
        };

        await _repository.Register(userModel);
    }
}
