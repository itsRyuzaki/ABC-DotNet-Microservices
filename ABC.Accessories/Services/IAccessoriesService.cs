using ABC.Accessories.DTO.Response;
using ABC.Accessories.Models;
using ABC.Accessories.Models.MongoDb;

namespace ABC.Accessories.Services;

public interface IAccessoriesService
{
    public Task<ApiResponseDto<string>> AddAccessoryAsync(Accessory accessory, string accessoryBaseId, string type);

    public Task<ApiResponseDto<string>> AddAccessoryExtrasAsync(AccessoryExtras accessoryExtras, string type);

    public Task<ApiResponseDto<string>> AddSellerAsync(Seller seller, string type);

    public Task<ApiResponseDto<string>> AddAccessoryBaseAsync(AccessoryBase accessoryBase, string type);
    
    public Task<ApiResponseDto<string>> AddAccessoryBaseExtrasAsync(AccessoryBaseExtras baseExtras, string type);

    public Task<List<Seller>> GetSellersAsync(int[] sellerIds, string type);

    public Task<Accessory?> GetAccessoryFromGuidAsync(string accessoryGuid, string type);

    public Task<ApiResponseDto<string>> AddImagesToAccessoryAsync(List<ItemImage> itemImages, Accessory accessory, string type);

}