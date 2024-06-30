using MessangerBack.Models;


namespace MessangerBack.Utils;

public interface IJwtProvider
{
    public string GenerateTokenByUserId(string userId);
}