using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class AccessoryBase
{
    public int Id { get; set; }

    [Required]
    public required string AccessoryBaseId { get; set; }

    public List<Accessory> Accessories { get; set; } = [];

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Category { get; set; }

    [Required]
    public required string SubCategory { get; set; }

    [Required]
    public required string Brand { get; set; }
}