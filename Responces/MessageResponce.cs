namespace MessangerBack.Responces;

public class MessageResponce
{
    public Guid Id { get; set; }

    public Guid SenderId { get; set; }

    public virtual UserResponce Sender{ get; set; }
    
    public string Text { get; set; }
    
    public DateTime MessageSentTime { get; set; }
}
