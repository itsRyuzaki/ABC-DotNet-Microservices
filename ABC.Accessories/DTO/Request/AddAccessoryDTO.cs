using System.ComponentModel.DataAnnotations;
using ABC.Accessories.Models;

namespace ABC.Accessories.DTO.Request;

public class ItemImageDTO
{
    [Required]
    public required string AltText { get; set; }

    [Required]
    public required string Source { get; set; }
}

public class AddAccessoryDTO
{
    [Required]
    public required string Type { get; set; }

    [Required]
    public required string AccessoryBaseId {get; set;}

    [Required]
    [MinLength(1)]
    public int[] SellerIds { get; set; } = [];

    [Required]
    [MinLength(1)]
    public List<ItemImageDTO> Images { get; set; } = [];

    [Required]
    public required string Description { get; set; }

    public string[] Specifications { get; set; } = [];

    [Required]
    public required string[] InBoxItems { get; set; }

    [Required]
    public Decimal SellerPrice { get; set; }

    [Required]
    public Decimal AbcPrice { get; set; }

    [Required]
    public int AvailableCount { get; set; }

}