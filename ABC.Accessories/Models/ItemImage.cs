using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class ItemImage
{
    public int Id { get; set; }

    public int AccessoryId { get; set; }

    public Accessory Accessory { get; set; } = null!;

    [Required]
    public required string AltText { get; set; }

    [Required]
    public required string Source { get; set; }

    [Required]
    public int Order { get; set; }

}