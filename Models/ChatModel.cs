using Microsoft.EntityFrameworkCore;


namespace MessangerBack.Models;

public class ChatModel
{
    public Guid Id { get; set; }

    public Guid AdminId { get; set; }

    public UserModel Admin{ get; set; }

    public Guid[] Users { get; set; }
    
    public string ChatName { get; set; }

    public Guid? LastMessageId { get; set; }

    public MessageModel? LastMessage { get; set; }

    public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
}