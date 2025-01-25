using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;

namespace ABC.Accessories.Facade;

public interface IAccessoriesFacade
{

    public Task<ApiResponseDto<string>> AddAccessoryDetailAsync(AddAccessoriesDTO requestPayload);
    
    public Task<ApiResponseDto<string>> AddSellerDetailsAsync(AddSellerDTO payload);


}