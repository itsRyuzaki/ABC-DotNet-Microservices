using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class AddAccessoryBaseDTO
{
    [Required]
    public required string Type { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int DeviceModelId { get; set; }

    [Required]
    public int BrandId { get; set; }

    public KeyValuePair<string, string[]>[] MasterData {get; set;} = [];

}