using ABC.Users.DTO;
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
        await _userService.AddUserAsync(userData);

        return Ok();
    }

}
