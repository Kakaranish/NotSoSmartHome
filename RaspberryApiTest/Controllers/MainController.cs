using System.Device.Gpio;
using Microsoft.AspNetCore.Mvc;
using RaspberryApiTest.Services;

namespace RaspberryApiTest.Controllers;

[Route("/")]
public class MainController : ControllerBase
{
    private readonly ILogger<MainController> _logger;
    private readonly GpioControllerAccessor _gpioControllerAccessor;
    private readonly PumpsProvider _pumpsProvider;

    public MainController(
        ILogger<MainController> logger,
        GpioControllerAccessor gpioControllerAccessor,
        PumpsProvider pumpsProvider)
    {
        _logger = logger;
        _gpioControllerAccessor = gpioControllerAccessor;
        _pumpsProvider = pumpsProvider;
    }

    [HttpGet]
    public string Get()
    {
        var now = DateTime.UtcNow;
        return $"Hello world at {now:o}";
    }

    [HttpPost("/toggle")]
    public IActionResult Toggle([FromBody] ToggleRequestDto requestDto)
    {
        var pump = _pumpsProvider.GetOrDefaultById(requestDto.PumpId);
        if (pump is null)
        {
            return NotFound("Pump not found");
        }
        
        var currentPinValue = _gpioControllerAccessor.GetPinValue(pump.Pin);
        var newPinValue = currentPinValue == PinValue.High ? PinValue.Low : PinValue.High;

        _gpioControllerAccessor.SetPinValue(pump.Pin, newPinValue);

        _logger.LogInformation("Toggled pin '{PinNumber}' value from '{PreviousPinValue}' to '{NewPinValue}'",
            pump.Pin, currentPinValue, newPinValue);

        return Ok();
    }

    public record ToggleRequestDto(string PumpId);
}