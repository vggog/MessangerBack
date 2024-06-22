using MessangerBack.Models;
using MessangerBack.Repositories;
using MessangerBack.Schemas;
using MessangerBack.Utils;
using MessangerBack.Exceptions;


namespace MessangerBack.Services;

public class AuthService : IAuthService
{
    IPasswordUtils _passwordUtils;
    IAuthRepository _repository;
    IJwtProvider _provider;

    public AuthService(IAuthRepository repository, IPasswordUtils passwordUtils, IJwtProvider provider)
    {
        _repository = repository;
        _passwordUtils = passwordUtils;
        _provider = provider;
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

    public async Task<string> LoginUser(LoginUserSchema userData)
    {
        UserModel? user = await _repository.GetUserByUsername(userData.UserName);
        if (user == null)
        {
            throw new NotFoundException("User with this username not found");
        }

        bool passwordIsCorrect = _passwordUtils.CheckPassword(userData.Password, user.PasswordHash);

        if (!passwordIsCorrect)
        {
            throw new PasswordIsIncorrectException("Password is incorrect.");
        }

        return _provider.GenerateToken(user);
    }
}
