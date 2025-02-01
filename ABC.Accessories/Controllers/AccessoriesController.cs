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

    [HttpPost("add/accessory-base")]
    public async Task<IActionResult> AddAccessoryBase(AddAccessoryBaseDTO payload){
        var response = await _accessoriesFacade.AddAccessoryBaseDetailAsync(payload);
        return StatusCode(
                    (int)(response.Success ? ResponseCode.SUCCESS_CREATED : ResponseCode.ERROR),
                    response
                );
    }

    [HttpPost("add/accessory")]
    public async Task<IActionResult> AddAccessory(AddAccessoryDTO payload)
    {
        var response = await _accessoriesFacade.AddAccessoryDetailAsync(payload);
        int responseCode = response.Success ?
                        (int)ResponseCode.SUCCESS_CREATED :
                        (response.ErrorDetails?.Code ?? (int)ResponseCode.ERROR);


        return StatusCode(responseCode, response);

    }

    [HttpPost("add/accessory/images")]
    public async Task<IActionResult> AddAccessoryImage(List<IFormFile> images, string type, string accessoryGuid){
        await _accessoriesFacade.AddAccessoryImageAsync(images, type, accessoryGuid);        
        return Ok();
    } 

    [HttpPost("add/seller")]
    public async Task<IActionResult> AddSeller(AddSellerDTO payload)
    {
        var response = await _accessoriesFacade.AddSellerDetailsAsync(payload);
        return StatusCode(
                    (int)(response.Success ? ResponseCode.SUCCESS_CREATED : ResponseCode.ERROR),
                    response
                );
    }
}
