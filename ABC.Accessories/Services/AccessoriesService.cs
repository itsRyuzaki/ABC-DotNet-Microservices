using ABC.Accessories.Constants;
using ABC.Accessories.Data;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Models;
using ABC.Accessories.Enums;
using Microsoft.EntityFrameworkCore;
using ABC.Accessories.Services.MongoDb;
using ABC.Accessories.Models.MongoDb;
using MongoDB.Driver;

namespace ABC.Accessories.Services;
public class AccessoriesService : IAccessoriesService
{

    private readonly Dictionary<string, AccessoriesDataContext> _contextMap = [];
    private readonly Dictionary<string, IMongoCollection<AccessoryExtras>> _accExtrasCollectionMap = [];
    private readonly Dictionary<string, IMongoCollection<AccessoryBaseExtras>> _baseExtrasCollectionMap = [];

    private readonly ILogger _logger;

    public AccessoriesService(
                ComputersDataContext computersDataContext,
                MobilesDataContext mobilesDataContext,
                ILogger<AccessoriesService> logger,
                IMongoDbService mongoDbService
            )
    {
        _accExtrasCollectionMap = mongoDbService.GetAccExtrasCollectionMap();
        _baseExtrasCollectionMap = mongoDbService.GetBaseExtrasCollectionMap();

        _contextMap.Add(AccessoriesType.Mobile, mobilesDataContext);
        _contextMap.Add(AccessoriesType.Computer, computersDataContext);
        _logger = logger;

    }


    public async Task<ApiResponseDto<string>> AddAccessoryAsync(
                                                            Accessory accessory,
                                                            string accessoryBaseId,
                                                            string type
                                                        )
    {
        try
        {
            var accessoryBaseDetail = await _contextMap[type].AccessoryBase
                            .SingleOrDefaultAsync(x => x.AccessoryBaseId == accessoryBaseId);

            if (accessoryBaseDetail == null)
            {
                _logger.LogError("No base details found for baseId: {baseId}", accessoryBaseId);
                throw new Exception($"No base details found for baseId: {accessoryBaseId}");
            }
            else
            {
                accessoryBaseDetail.Accessories.Add(accessory);
                await _contextMap[type].SaveChangesAsync();
                _logger.LogInformation("Saved details for accessory: {name}", accessoryBaseId);
            }

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving details for accessory: {name}. See error stack below: \n {error}",
                        accessoryBaseId,
                        error.ToString()
                    );
            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving accessory details."]);
        }

        return ApiResponseDto.HandleSuccessResponse(accessory.AccessoryGuid);

    }

    public async Task<ApiResponseDto<string>> AddAccessoryExtrasAsync(AccessoryExtras accessoryExtras, string type)
    {
        try
        {
            await _accExtrasCollectionMap[type].InsertOneAsync(accessoryExtras);

            _logger.LogInformation("Saved extra details for Accessory: {name}", accessoryExtras.AccessoryGuid);
            return ApiResponseDto.HandleSuccessResponse("Accessory Extra Added");

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving extra details for accessory: {name}. See error stack below: \n {error}",
                        accessoryExtras.AccessoryGuid,
                        error.ToString()
                    );

            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving extra details for accessory."]);
        }


    }

    public async Task<ApiResponseDto<string>> AddAccessoryBaseAsync(AccessoryBase accessoryBase, string type)
    {
        try
        {
            _contextMap[type].AccessoryBase.Add(accessoryBase);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved base details for Accessory: {name}", accessoryBase.Name);
            return ApiResponseDto.HandleSuccessResponse("Base Accessory Added");

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving base details for accessory: {name}. See error stack below: \n {error}",
                        accessoryBase.Name,
                        error.ToString()
                    );

            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving base details for accessory."]);
        }


    }

    public async Task<ApiResponseDto<string>> AddAccessoryBaseExtrasAsync(AccessoryBaseExtras baseExtras, string type)
    {
        try
        {
            await _baseExtrasCollectionMap[type].InsertOneAsync(baseExtras);

            _logger.LogInformation("Saved extra base details for Accessory: {name}", baseExtras.AccessoryBaseId);
            return ApiResponseDto.HandleSuccessResponse("Extra Accessory Base Added");

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving extra base details for accessory: {name}. See error stack below: \n {error}",
                        baseExtras.AccessoryBaseId,
                        error.ToString()
                    );

            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving extra base details for accessory."]);
        }


    }

    public async Task<ApiResponseDto<string>> AddSellerAsync(Seller seller, string type)
    {

        try
        {
            _contextMap[type].Sellers.Add(seller);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved details for seller: {name}", seller.Name);
            return ApiResponseDto.HandleSuccessResponse("Seller Added");

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving details for seller: {name}. See error stack below: \n {error}",
                        seller.Name,
                        error.ToString()
                    );

            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving seller details."]);
        }


    }

    public async Task<List<Seller>> GetSellersAsync(int[] sellerIds, string type)
    {
        try
        {
            _logger.LogInformation("Fetching seller details...");

            return await _contextMap[type].Sellers
                                            .Where(seller => sellerIds.Contains(seller.Id))
                                            .ToListAsync();
        }
        catch (Exception error)
        {
            _logger.LogError("Error while fetching seller details. See error stack below: \n {error}", error.ToString());
            return [];
        }
    }

    public async Task<Accessory?> GetAccessoryFromGuidAsync(string accessoryGuid, string type)
    {
        try
        {
            _logger.LogInformation("Fetching Accessory details from Guid: {guid}", accessoryGuid);

            return await _contextMap[type].Accessories
                                             .FirstOrDefaultAsync(x => x.AccessoryGuid == accessoryGuid);

        }
        catch (Exception error)
        {
            _logger.LogError("Error while fetching Accessory details from Guid: {guid}. See error stack below: \n {error}", accessoryGuid, error.ToString());
            return null;
        }
    }

    public async Task<ApiResponseDto<string>> AddImagesToAccessoryAsync(List<ItemImage> itemImages, Accessory accessory, string type)
    {
        try
        {
            accessory.Images.AddRange(itemImages);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved images for Accessory: {guid}", accessory.AccessoryGuid);
            return ApiResponseDto.HandleSuccessResponse("Added Images to Accessory Details");

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving images for accessory: {guid}. See error stack below: \n {error}",
                        accessory.AccessoryGuid,
                        error.ToString()
                    );

            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving images for accessory."]);
        }

    }

}