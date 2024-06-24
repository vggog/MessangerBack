using System.ComponentModel.DataAnnotations;


namespace MessangerBack.Schemas;

public class CreateChatRequestSchema
{
    [Required]
    public string ChatName { get; set; }
}