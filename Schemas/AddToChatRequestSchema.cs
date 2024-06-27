using System.ComponentModel.DataAnnotations;


namespace MessangerBack.Schemas;

public class AddToChatRequestSchema
{
    [Required]
    public string ChatId { get; set; }
}
