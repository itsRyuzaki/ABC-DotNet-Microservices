using ABC.Users.DTO;
using ABC.Users.Enums;
using ABC.Users.Facade;
using Microsoft.AspNetCore.Mvc;

namespace ABC.Users.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserFacade _userFacade) : ControllerBase
{

    [HttpGet("Health", Name = "GetHealth")]
    public string GetHealthStatus()
    {
        return "Users microservice up and running!!";
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUpUser(UserSignUpDto userData)
    {
        var response = await _userFacade.SignUpUserAsync(userData);

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

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(UserLoginDto loginRequest)
    {
        var response = await _userFacade.LoginUserAsync(loginRequest);
        return response.Success ? Ok(): Unauthorized();
    }

}
