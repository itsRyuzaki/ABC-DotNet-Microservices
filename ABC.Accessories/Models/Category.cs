using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    public required string Type { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }
}