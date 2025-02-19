using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class BaseImageDetail
{
    [Required]
    public required string AltText { get; set; }

    [Required]
    public required string Source { get; set; }

    [Required]
    public int Order { get; set; }
}