using ABC.Accessories.DTO.Response;
using ABC.Accessories.Models;

namespace ABC.Accessories.Services;

public interface IAccessoriesService
{
    public Task<ApiResponseDto<string>> AddAccessoryAsync(Accessory accessory, string type);

    public Task<ApiResponseDto<string>> AddSellerAsync(Seller seller, string type);

    public Task<List<Seller>> GetSellersAsync(int[] sellerIds, string type);

}