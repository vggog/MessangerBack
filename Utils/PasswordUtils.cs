namespace MessangerBack.Utils;


public class PasswordUtils : IPasswordUtils
{
    public string GeneratePasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }
}
