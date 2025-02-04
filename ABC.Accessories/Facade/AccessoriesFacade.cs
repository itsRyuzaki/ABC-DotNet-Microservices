using ABC.Accessories.Constants;
using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Enums;
using ABC.Accessories.Helpers;
using ABC.Accessories.Models;
using ABC.Accessories.Services;
using ABC.Accessories.Services.Blob;
using AutoMapper;

namespace ABC.Accessories.Facade;

public class AccessoriesFacade(
                    IMapper _mapper,
                    IAccessoriesService _accessoriesService,
                    IBlobService _blobService,
                    IAccessoriesHelper _accessoriesHelper,
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

    public async Task<ApiResponseDto<List<bool>>> AddAccessoryImagesAsync(List<IFormFile> images, IFormFile requestPayload)
    {
        var deserializedResponse = await _accessoriesHelper
                                                .DeserializeJsonFromFileAsync<AddAccessoryImagesDTO>(requestPayload);

        if (!deserializedResponse.Success || deserializedResponse.Data == null)
        {
            return new ApiResponseDto<List<bool>>()
            {
                Success = false,
                ErrorDetails = deserializedResponse.ErrorDetails
            };
        }

        var itemImagesPayload = deserializedResponse.Data;
        var type = itemImagesPayload.Type;
        var accessoryGuid = itemImagesPayload.AccessoryGuid;

        var accessory = await _accessoriesService.GetAccessoryFromGuidAsync(accessoryGuid, type);

        if (accessory == null)
        {
            return ApiResponseDto<List<bool>>.HandleErrorResponse(
                                            (int)ResponseCode.BAD_REQUEST,
                                            ["No Accessory details found for given guid"]
                                        );
        }


        for (int i = 0; i < itemImagesPayload.ItemImages.Count; i++)
        {
            itemImagesPayload.ItemImages[i].File = images[i];

        }

        return await SaveImagesToBlobAndDbAsync(itemImagesPayload, type, accessory);

    }

    private async Task<ApiResponseDto<List<bool>>> SaveImagesToBlobAndDbAsync(AddAccessoryImagesDTO itemImagesPayload, string type, Accessory accessory)
    {
        List<bool> fileSavedResponse = [];
        List<ItemImage> savedItemImages = [];


        await Parallel.ForEachAsync(
                itemImagesPayload.ItemImages,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                async (itemImageDTO, CancellationToken) =>
                {
                    var fileName = _accessoriesHelper.SanitizeBlobName(itemImageDTO.File.FileName);
                    var filePath = $"{BlobPath.ItemImages}/{accessory.AccessoryGuid}/{fileName}";

                    var fileSaved = await _blobService.Upload(
                                            type.ToLower(),
                                            filePath,
                                            itemImageDTO.File
                                        );
                    if (fileSaved)
                    {
                        var itemImage = _mapper.Map<ItemImage>(itemImageDTO);
                        itemImage.Source = filePath;
                        savedItemImages.Add(itemImage);

                    }
                    fileSavedResponse.Add(fileSaved);
                });


        if (savedItemImages.Count != 0)
        {
            var savedResponse = await _accessoriesService.AddImagesToAccessoryAsync(savedItemImages, accessory, type);
            if (savedResponse.Success)
            {
                return ApiResponseDto<List<bool>>.HandleSuccessResponse(fileSavedResponse);
            }
        }


        return ApiResponseDto<List<bool>>.HandleErrorResponse(
                                                            (int)ResponseCode.ERROR,
                                                            ["Error while saving images"]
                                                        );
    }
}