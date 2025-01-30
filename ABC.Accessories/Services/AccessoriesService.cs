using ABC.Accessories.Constants;
using ABC.Accessories.Data;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Models;
using ABC.Accessories.Enums;
using Microsoft.EntityFrameworkCore;

namespace ABC.Accessories.Services;
public class AccessoriesService : IAccessoriesService
{

    private readonly Dictionary<string, AccessoriesDataContext> _contextMap = [];
    private readonly ILogger _logger;

    public AccessoriesService(
                ComputersDataContext computersDataContext,
                MobilesDataContext mobilesDataContext,
                ILogger<AccessoriesService> logger
            )
    {
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
                throw new Exception($"No base details found for baseId: {accessoryBaseId}" );
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

        return ApiResponseDto.HandleSuccessResponse("Accessory Added!");

    }

    public async Task<ApiResponseDto<string>> AddAccessoryBaseAsync(AccessoryBase accessoryBase, string type)
    {
        try
        {
            _contextMap[type].AccessoryBase.Add(accessoryBase);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved base details for Accessory: {name}", accessoryBase.Name);
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

        return ApiResponseDto.HandleSuccessResponse("Base Accessory Added");

    }


    public async Task<ApiResponseDto<string>> AddSellerAsync(Seller seller, string type)
    {

        try
        {
            _contextMap[type].Sellers.Add(seller);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved details for seller: {name}", seller.Name);
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

        return ApiResponseDto.HandleSuccessResponse("Seller Added");

    }

    public async Task<List<Seller>> GetSellersAsync(int[] sellerIds, string type)
    {
        return await _contextMap[type].Sellers
                                        .Where(seller => sellerIds.Contains(seller.Id))
                                        .ToListAsync();
    }




}