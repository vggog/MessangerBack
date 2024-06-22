namespace MessangerBack.Utils;

public interface IPasswordUtils
{
    public string GeneratePasswordHash(string password);

    public bool CheckPassword(string password, string passwordHash);
}
