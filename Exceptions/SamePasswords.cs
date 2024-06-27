namespace MessangerBack.Exceptions;

[Serializable]
public class SamePasswords : Exception
{
    public SamePasswords(string message) 
        : base(message) { }
}
