
namespace ABC.Accessories.Models;

public class ItemImage : BaseImageDetail
{
    public int Id { get; set; }

    public int AccessoryId { get; set; }

    public Accessory Accessory { get; set; } = null!;

}