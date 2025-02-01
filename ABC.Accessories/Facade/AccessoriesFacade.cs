using System.Text.RegularExpressions;
using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Enums;
using ABC.Accessories.Models;
using ABC.Accessories.Services;
using ABC.Accessories.Services.Blob;
using AutoMapper;

namespace ABC.Accessories.Facade;

public class AccessoriesFacade(
                    IMapper _mapper,
                    IAccessoriesService _accessoriesService,
                    IBlobService _blobService,
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
            accessoryDetail.AccessoryGuid = $"{payload.AccessoryBaseId}-{Guid.NewGuid()}";
            accessoryDetail.Inventory = new Inventory()
            {
                AvailableCount = payload.AvailableCount,
                TotalSold = 0
            };

            var dateTime = DateTime.UtcNow;
            accessoryDetail.CreatedDate = dateTime;
            accessoryDetail.UpdatedDate = dateTime;

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
        var details = _mapper.Map<AccessoryBase>(payload);

        details.AccessoryBaseId = Guid.NewGuid().ToString();

        return await _accessoriesService.AddAccessoryBaseAsync(details, payload.Type);
    }

    public async Task<ApiResponseDto<string>> AddSellerDetailsAsync(AddSellerDTO payload)
    {
        return await _accessoriesService.AddSellerAsync(_mapper.Map<Seller>(payload), payload.Type);
    }

    public async Task<ApiResponseDto<string>> AddAccessoryImageAsync(List<IFormFile> images, string type, string accessoryGuid)
    {

        await Parallel.ForEachAsync(images, async (image, CancellationToken) =>
        {
            await _blobService.Upload(type, $"item-images/{accessoryGuid}/{SanitizeBlobName(image.FileName)}", image);
        });

        return ApiResponseDto.HandleSuccessResponse("");

    }

    public string SanitizeBlobName(string fileName)
    {
        // Regex to replace any character that is not a-z, 0-9, or hyphen with a hyphen
        return Regex.Replace(fileName.ToLower(), @"[^a-z0-9\-]", "-");
    }
}