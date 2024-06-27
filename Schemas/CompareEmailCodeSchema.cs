using System.ComponentModel.DataAnnotations;

namespace MessangerBack.Schemas;

public class CompareEmailCodeSchema
{
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? CodeToCompareWith { get; set; }
}
