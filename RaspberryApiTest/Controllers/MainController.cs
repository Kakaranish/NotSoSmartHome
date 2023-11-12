using System.Device.Gpio;
using Microsoft.AspNetCore.Mvc;

namespace RaspberryApiTest.Services;

[Route("/")]
public class MainController : ControllerBase
{
    private readonly ILogger<MainController> _logger;
    private readonly GpioController _gpioController;
    private readonly PumpsProvider _pumpsProvider;

    public MainController(
        ILogger<MainController> logger,
        GpioController gpioController,
        PumpsProvider pumpsProvider)
    {
        _logger = logger;
        _gpioController = gpioController;
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
            return NotFound("Pump not found");
        
        var currentPinValue = _gpioController.Read(pump.Pin);
        var newPinValue = currentPinValue == PinValue.High ? PinValue.Low : PinValue.High;

        _gpioController.Write(pump.Pin, newPinValue);

        _logger.LogInformation("Toggled pin '{PinNumber}' value from '{PreviousState}' to '{NewState}'",
            pump.Pin, currentPinValue, newPinValue);

        return Ok();
    }

    public record ToggleRequestDto(string PumpId);
}