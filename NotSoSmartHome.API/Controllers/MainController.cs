using Microsoft.AspNetCore.Mvc;

namespace NotSoSmartHome.API.Controllers;

[Route("/")]
public class MainController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        var now = DateTime.UtcNow;
        return $"Hello world at {now:o}";
    }
}