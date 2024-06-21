namespace MessangerBack.Utils;

public interface IPasswordUtils
{
    public string GeneratePasswordHash(string password);
}
