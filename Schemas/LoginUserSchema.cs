using System.ComponentModel.DataAnnotations;


namespace MessangerBack.Schemas;

public class LoginUserSchema
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Password { get; set; }
}