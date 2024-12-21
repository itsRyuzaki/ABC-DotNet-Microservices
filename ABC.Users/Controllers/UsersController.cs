using ABC.Users.DTO;
using ABC.Users.Enums;
using ABC.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC.Users.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("Health", Name = "GetHealth")]
    public string GetHealthStatus()
    {
        return "Users microservice up and running!!";
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserSignUpDto userData)
    {
        var response = await _userService.AddUserAsync(userData);

        if (response.ErrorDetails?.Code == (int)ResponseCode.DUPLICATE)
        {

            return Conflict(response);
        }
        else if (!response.Success)
        {
            return StatusCode(450, response);
        }
        else
        {
            return Ok(response);
        }
    }

}
