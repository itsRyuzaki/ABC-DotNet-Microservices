using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class Accessory
{
    public int Id { get; set; }
    public List<Seller> Sellers { get; set; } = [];

    public List<ItemImage> Images { get; set; } = [];

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }
    
    [Required]
    public Decimal SellerPrice { get; set; }

    [Required]
    public Decimal AbcPrice { get; set; }

    [Required]
    public required string Category { get; set; }

    [Required]
    public required string SubCategory { get; set; }

    [Required]
    public required string Brand { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

}
