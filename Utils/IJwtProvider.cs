using MessangerBack.Models;


namespace MessangerBack.Utils;

public interface IJwtProvider
{
    public string GenerateToken(UserModel user);
}