using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace MessangerBack.Models;

public class ChatModel
{
    public Guid Id { get; set; }

    public Guid AdminId { get; set; }

    public virtual IdentityUser<Guid> Admin{ get; set; }

    public Guid[] Users { get; set; }
    
    public virtual List<IdentityUser<Guid>> ModelsOfUsers { get; set; }
    
    public string ChatName { get; set; }

    public Guid? LastMessageId { get; set; }

    public virtual MessageModel? LastMessage { get; set; }

    public virtual List<MessageModel> Messages { get; set; } = new List<MessageModel>();
}