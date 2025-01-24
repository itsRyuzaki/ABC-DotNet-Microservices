using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Enums;
using ABC.Accessories.Models;
using ABC.Accessories.Services;
using AutoMapper;

namespace ABC.Accessories.Facade;

public class AccessoriesFacade(IAccessoriesService _accessoriesService, IMapper _mapper) : IAccessoriesFacade
{
    public async Task<ApiResponseDto<string>> AddAccessoryDetailAsync(AddAccessoriesDTO requestPayload)
    {
        var accessoryDetail = _mapper.Map<Accessory>(requestPayload);

        // get seller details from seller ids
        var sellers = await _accessoriesService
                                    .GetSellersAsync(requestPayload.SellerIds, requestPayload.Type);


        if (sellers != null && sellers.Count != 0)
        {
            accessoryDetail.Sellers = sellers;
            return await _accessoriesService
                                 .AddAccessoryAsync(accessoryDetail, requestPayload.Type);
        }
        else
        {
            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.BAD_REQUEST, ["Invalid SellerIds"]);
        }
    }
}