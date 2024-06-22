namespace MessangerBack.Exceptions;

[Serializable]
public class PasswordIsIncorrectException : Exception
{ 
    public PasswordIsIncorrectException(string message)
        : base(message) { }
}
