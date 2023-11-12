using System.Device.Gpio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RaspberryApiTest.Configuration;

namespace RaspberryApiTest.Services;

[Route("/")]
public class MainController : ControllerBase
{
    private readonly ILogger<MainController> _logger;
    private readonly GpioController _gpioController;
    private readonly IOptionsMonitor<RaspberryOptions> _raspberryOptions;

    public MainController(
        ILogger<MainController> logger,
        GpioController gpioController,
        IOptionsMonitor<RaspberryOptions> raspberryOptions)
    {
        _logger = logger;
        _gpioController = gpioController;
        _raspberryOptions = raspberryOptions;
    }

    [HttpGet]
    public string Get()
    {
        var now = DateTime.UtcNow;
        return $"Hello world at {now:o}";
    }

    [HttpPost("/toggle")]
    public void Toggle()
    {
        var pumpPin = _raspberryOptions.CurrentValue.PumpPin;
        var currentPinValue = _gpioController.Read(pumpPin);
        var newPinValue = currentPinValue == PinValue.High ? PinValue.Low : PinValue.High;

        _gpioController.Write(pumpPin, newPinValue);

        _logger.LogInformation("Toggled pin '{PinNumber}' value from '{PreviousState}' to '{NewState}'",
            pumpPin, currentPinValue, newPinValue);
    }
}