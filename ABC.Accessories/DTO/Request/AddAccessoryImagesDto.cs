using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;
public class ItemImageDTO
{
    public IFormFile File { get; set; } = null!;

    [Required]
    public required string AltText { get; set; }

    [Required]
    public required int Order { get; set; }
}
public class AddAccessoryImagesDTO
{
    public required string Type { get; set; }
    public required string AccessoryGuid { get; set; }

    [Required]
    [MinLength(1)]
    public List<ItemImageDTO> ItemImages { get; set; } = [];
}
