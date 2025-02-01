using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;

namespace ABC.Accessories.Facade;

public interface IAccessoriesFacade
{

    public Task<ApiResponseDto<string>> AddAccessoryDetailAsync(AddAccessoryDTO requestPayload);

    public Task<ApiResponseDto<string>> AddAccessoryBaseDetailAsync(AddAccessoryBaseDTO payload);
    
    public Task<ApiResponseDto<string>> AddSellerDetailsAsync(AddSellerDTO payload);

    public Task<ApiResponseDto<string>> AddAccessoryImageAsync(List<IFormFile> images, string type, string accessoryGuid);

}