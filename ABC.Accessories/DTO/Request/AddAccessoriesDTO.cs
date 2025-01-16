using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class AccessoryDetail {
    [Required]
    public required string Name { get; set; }

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
    public required string Category { get; set; }

    [Required]
    public required string SubCategory { get; set; }

    [Required]
    public required string Brand { get; set; }
}

public class AddAccessoriesDTO {
    [Required]
    public required string Type {get; set;}

    [Required]
    public required List<AccessoryDetail> AccessoryDetails {get; set;}
}