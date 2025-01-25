using ABC.Accessories.DTO.Request;
using ABC.Accessories.Enums;
using ABC.Accessories.Facade;
using Microsoft.AspNetCore.Mvc;

namespace ABC.Accessories.Controllers;

[ApiController]
[Route("[controller]")]
public class AccessoriesController(IAccessoriesFacade _accessoriesFacade) : ControllerBase
{
    [HttpGet("Health", Name = "GetHealth")]
    public string GetHealthStatus()
    {
        return "Accessories microservice up and running!!";
    }

    [HttpPost("add/accessory")]
    public async Task<IActionResult> AddAccessories(AddAccessoriesDTO payload)
    {
        var response = await _accessoriesFacade.AddAccessoryDetailAsync(payload);
        int responseCode = response.Success ?
                        (int)ResponseCode.SUCCESS_CREATED :
                        (response.ErrorDetails?.Code ?? (int)ResponseCode.ERROR);


        return StatusCode(responseCode, response);

    }

    [HttpPost("add/seller")]
    public async Task<IActionResult> AddSellers(AddSellerDTO payload)
    {
        var response = await _accessoriesFacade.AddSellerDetailsAsync(payload);
        return StatusCode(
                    (int)(response.Success ? ResponseCode.SUCCESS_CREATED : ResponseCode.ERROR),
                    response
                );
    }
}
