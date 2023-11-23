using Microsoft.AspNetCore.Mvc;
using NotSoSmartHome.Services;

namespace NotSoSmartHome.Controllers;

[Route("/")]
public class MainController : ControllerBase
{
    public MainController()
    {
    }

    [HttpGet]
    public string Get()
    {
        var now = DateTime.UtcNow;
        return $"Hello world at {now:o}";
    }

    public record ToggleRequestDto(string PumpId);
}