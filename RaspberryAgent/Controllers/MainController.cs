using System.Device.Gpio;
using Microsoft.AspNetCore.Mvc;

namespace RaspberryAgent.Controllers;

[ApiController]
[Route("")]
public class MainController : ControllerBase
{
    private readonly GpioControllerAccessor _gpioControllerAccessor;

    public MainController(GpioControllerAccessor gpioControllerAccessor)
    {
        _gpioControllerAccessor = gpioControllerAccessor;
    }

    [HttpPost("")]
    public IActionResult PostSetPinValue([FromBody] SetPinValueRequestDto dto)
    {
        return Ok(dto.PinValue);
    }
    
    public record SetPinValueRequestDto(int Pin, PinValue PinValue, Guid? LeaseId = null);
}
