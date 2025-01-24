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

    public AccessoriesService(ComputersDataContext computersDataContext, MobilesDataContext mobilesDataContext)
    {
        _contextMap.Add(AccessoriesType.Mobile, mobilesDataContext);
        _contextMap.Add(AccessoriesType.Computer, computersDataContext);

    }


    public async Task<ApiResponseDto<string>> AddAccessoryAsync(Accessory accessory, string type)
    {

        try
        {
            _contextMap[type].Accessories.Add(accessory);
            await _contextMap[type].SaveChangesAsync();
        }
        catch (Exception error)
        {
            Console.WriteLine(error);
            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving accessory details."]);
        }

        return ApiResponseDto.HandleSuccessResponse("");

    }

    public async Task<List<Seller>> GetSellersAsync(int[] sellerIds, string type)
    {
        return await _contextMap[type].Sellers
                                        .Where(seller => sellerIds.Contains(seller.Id))
                                        .ToListAsync();
    }




}