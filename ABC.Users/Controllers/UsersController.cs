using ABC.Users.DTO.Request;
using ABC.Users.Enums;
using ABC.Users.Facade;
using Microsoft.AspNetCore.Mvc;

namespace ABC.Users.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserFacade _userFacade) : ControllerBase
{

    [HttpGet("Health", Name = "GetHealth")]
    public IActionResult GetHealthStatus()
    {
        return Ok("Users microservice up and running!!");
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
            await AddSessionCookie(userData.UserName);
            return Ok(response);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(UserLoginDto loginRequest)
    {
        var response = await _userFacade.LoginUserAsync(loginRequest);

        if (response.Success)
        {
            await AddSessionCookie(loginRequest.UserName);
            return Ok(response);
        }


        return StatusCode(response.ErrorDetails?.Code ?? 401, response);
    }

    private async Task AddSessionCookie(string userName)
    {
        string token = await _userFacade.CreateSessionHistory(userName);
        Response.Cookies.Append("session_abc", token, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(30), // Expire in 30 mins
            IsEssential = true, // Makes the cookie essential for the app
            HttpOnly = true, // Makes the cookie accessible only via HTTP requests (not JavaScript)
            Secure = true, // Ensures the cookie is only sent over HTTPS
            SameSite = SameSiteMode.Strict // Restrict cookie sending to same-site requests
        });
    }

}
