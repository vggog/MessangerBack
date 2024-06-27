using System.ComponentModel.DataAnnotations;

namespace MessangerBack.Schemas;

public class SendEmailCodeSchema
{
    [Required]
    public string? Email { get; set; } 
}
