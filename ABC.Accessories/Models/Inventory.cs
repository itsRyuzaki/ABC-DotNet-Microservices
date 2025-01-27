
using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class Inventory
{
    public int Id { get; set; }

    public int AccessoryId { get; set; }

    public Accessory Accessory { get; set; } = null!;

    [Required]
    public int AvailableCount { get; set; }

    [Required]
    public int TotalSold { get; set; }
}