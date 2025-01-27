using System.ComponentModel.DataAnnotations;
using ABC.Accessories.Models;

namespace ABC.Accessories.DTO.Request;


public class AddAccessoryBaseDTO
{
    [Required]
    public required string Type { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Category { get; set; }

    [Required]
    public required string SubCategory { get; set; }

    [Required]
    public required string Brand { get; set; }
}