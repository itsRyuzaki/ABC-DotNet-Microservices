using Microsoft.AspNetCore.Mvc;

namespace ABC.Users.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
   

    [HttpGet(Name = "GetHealth")]
    public string Get()
    {
        return "Users microservice up and running!!";
    }
}
