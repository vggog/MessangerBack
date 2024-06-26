namespace MessangerBack.Responces;

public class AllChatsResponce
{
    public Guid Id { get; set; }
    
    public string ChatName { get; set; }

    public Guid? LastMessageId { get; set; }

    public virtual MessageResponce LastMessage { get; set; }
}
