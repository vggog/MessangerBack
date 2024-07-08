using System.ComponentModel.DataAnnotations;

namespace MessangerBack.Schemas;

public class ChangePasswordSchema
{
    [Required]
    public string? ResetCode { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? NewPassword { get; set; }
}
