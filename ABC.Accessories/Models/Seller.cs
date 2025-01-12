using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class Seller
{
    public int Id { get; set; }

    public List<Accessory> Accessories { get; set; } = [];

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string MobileNumber { get; set; }

    [Required]
    public required string Address { get; set; }

    public string? Website { get; set; }


}