using Microsoft.AspNetCore.Mvc;

namespace ABC_Microservices.NET.Controllers;

[ApiController]
[Route("[controller]")]
public class AbcController : ControllerBase
{

    [HttpGet(Name = "Health")]
    public string Get()
    {
        return "ABC Microservices is Up";
    }
}
