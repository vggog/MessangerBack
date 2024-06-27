namespace MessangerBack.Models;

public class MessageModel
{
    public Guid Id { get; set; }

    public Guid ChatId { get; set; }
    
    public virtual ChatModel Chat{ get; set; }

    public Guid SenderId { get; set; }

    public virtual UserModel Sender{ get; set; }
    
    public string Text { get; set; }
    
    public DateTime MessageSentTime { get; set; }
}
