using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Enums;
using ABC.Accessories.Models;
using ABC.Accessories.Services;
using AutoMapper;

namespace ABC.Accessories.Facade;

public class AccessoriesFacade(
                    IAccessoriesService _accessoriesService,
                    IMapper _mapper,
                    ILogger<AccessoriesFacade> _logger
                ) : IAccessoriesFacade
{
    public async Task<ApiResponseDto<string>> AddAccessoryDetailAsync(AddAccessoryDTO payload)
    {
        var accessoryDetail = _mapper.Map<Accessory>(payload);

        _logger.LogInformation("Fetching Seller Details for accessory: {name}", payload.AccessoryBaseId);

        // get seller details from seller ids
        var sellers = await _accessoriesService
                                    .GetSellersAsync(payload.SellerIds, payload.Type);


        if (sellers != null && sellers.Count != 0)
        {
            accessoryDetail.Sellers = sellers;
            accessoryDetail.Inventory = new Inventory()
            {
                AvailableCount = payload.AvailableCount,
                TotalSold = 0
            };

            accessoryDetail.CreatedDate = DateTime.UtcNow;
            accessoryDetail.UpdatedDate = DateTime.UtcNow;

            _logger.LogInformation("Saving Accessory Details for accessory: {name}", payload.AccessoryBaseId);

            return await _accessoriesService
                                 .AddAccessoryAsync(accessoryDetail, payload.AccessoryBaseId, payload.Type);
        }
        else
        {
            _logger.LogError("Invalid SellerIds for accessory: {name}", payload.AccessoryBaseId);
            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.BAD_REQUEST, ["Invalid SellerIds"]);
        }
    }

    public async Task<ApiResponseDto<string>> AddAccessoryBaseDetailAsync(AddAccessoryBaseDTO payload)
    {
        return await _accessoriesService.AddAccessoryBaseAsync(_mapper.Map<AccessoryBase>(payload), payload.Type);
    }

    public async Task<ApiResponseDto<string>> AddSellerDetailsAsync(AddSellerDTO payload)
    {
        return await _accessoriesService.AddSellerAsync(_mapper.Map<Seller>(payload), payload.Type);
    }
}