namespace MessangerBack.Exceptions;


[Serializable]
public class Forbidden : Exception
{
    public Forbidden(string message)
        : base(message) { }
}
