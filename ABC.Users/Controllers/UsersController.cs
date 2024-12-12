using ABC.Users.Models;
using ABC.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC.Users.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly MongoDbService _dbService;

    public UsersController(MongoDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("Health", Name = "GetHealth")]
    public string GetHealthStatus()
    {
        return "Users microservice up and running!!";
    }

     [HttpGet]
    public async Task<List<User>> Get() =>
        await _dbService.GetAsync();

    [HttpPost]
    public async Task<IActionResult> Post(User userData)
    {
        await _dbService.CreateAsync(userData);

        return Ok();
    }

}
