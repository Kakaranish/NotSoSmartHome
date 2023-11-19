using Microsoft.AspNetCore.Mvc;

namespace RaspberryAgent.API;

[Route("")]
public class MainController : Controller
{
    [HttpGet]
    public IActionResult SetPinValue()
    {
        return Ok("Hello world");
    }
}