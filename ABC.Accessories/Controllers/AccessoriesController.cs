using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;
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
    public async Task<IActionResult> AddAccessoryBase(AddAccessoryBaseDTO payload)
    {
        var response = await _accessoriesFacade.AddAccessoryBaseDetailAsync(payload);
        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);
    }

    [HttpPost("add/accessory")]
    public async Task<IActionResult> AddAccessory(AddAccessoryDTO payload)
    {
        var response = await _accessoriesFacade.AddAccessoryDetailAsync(payload);

        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);


    }

    [HttpPost("add/accessory/images")]
    public async Task<IActionResult> AddAccessoryImages(List<IFormFile> images, IFormFile requestPayload)
    {
        var response = await _accessoriesFacade.AddAccessoryImagesAsync(images, requestPayload);
        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);
    }

    [HttpPost("add/seller")]
    public async Task<IActionResult> AddSeller(AddSellerDTO payload)
    {
        var response = await _accessoriesFacade.AddSellerDetailsAsync(payload);
        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);
    }

    private ObjectResult GetStatusCode<T>(ApiResponseDto<T> response, ResponseCode successCode)
    {
        return StatusCode(
                   response.Success ? (int)successCode : (response.ErrorDetails?.Code ?? (int)ResponseCode.ERROR),
                   response
               );
    }
}
