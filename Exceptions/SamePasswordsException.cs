namespace MessangerBack.Exceptions;

[Serializable]
public class SamePasswordsException : Exception
{
    public SamePasswordsException(string message) 
        : base(message) { }
}
