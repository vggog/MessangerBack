namespace MessangerBack.Exceptions;


[Serializable]
public class WrongUserInputException : Exception
{
    public WrongUserInputException(string message)
        : base(message) { }
}
