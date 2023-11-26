using Microsoft.AspNetCore.Mvc;
using RaspberryAgent.Gpio;

namespace RaspberryAgent.Controllers;

[ApiController]
[ApiKeyAuthFilter]
public class MainController : ControllerBase
{
    private readonly GpioControllerAccessor _gpioControllerAccessor;

    public MainController(GpioControllerAccessor gpioControllerAccessor)
    {
        _gpioControllerAccessor = gpioControllerAccessor;
    }

    [HttpPost("pins")]
    public IActionResult PostSetPinValue([FromBody] SetPinValueRequestDto dto)
    {
        var result = _gpioControllerAccessor.SetPinValue(dto.Pin, dto.PinValue, dto.LeaseId);
        if (result == SetPinValueResult.Success)
            return Ok();
        if (result == SetPinValueResult.PinNotOpen)
            return BadRequest("Pin is not open");
        if (result == SetPinValueResult.InvalidLeaseId)
            return BadRequest("Invalid or missing lease id in request");
        
        return Ok();
    }
    
    public record SetPinValueRequestDto(int Pin, PinValue PinValue, Guid? LeaseId = null);
}
