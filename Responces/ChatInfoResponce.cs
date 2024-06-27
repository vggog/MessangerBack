namespace MessangerBack.Responces;

public class ChatInfoResponce
{
    public Guid Id { get; set; }
    
    public string ChatName { get; set; }

    public Guid AdminId { get; set; }

    public UserResponce Admin{ get; set; }

    public Guid[] Users { get; set; }

    public List<UserResponce> ModelsOfUsers { get; set; }

    public Guid? LastMessageId { get; set; }

    public MessageResponce LastMessage { get; set; }
}

