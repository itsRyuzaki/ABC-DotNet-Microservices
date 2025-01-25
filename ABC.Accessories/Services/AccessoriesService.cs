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


    public async Task<ApiResponseDto<string>> AddAccessoryAsync(Accessory accessory, string type)
    {
        try
        {
            _contextMap[type].Accessories.Add(accessory);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved details for accessory: {name}", accessory.Name);
        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving details for accessory: {name}. See error stack below: \n {error}",
                        accessory.Name,
                        error.ToString()
                    );
            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving accessory details."]);
        }

        return ApiResponseDto.HandleSuccessResponse("Accessry Added!");

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