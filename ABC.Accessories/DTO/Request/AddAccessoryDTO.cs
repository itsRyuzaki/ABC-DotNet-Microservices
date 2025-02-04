using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

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