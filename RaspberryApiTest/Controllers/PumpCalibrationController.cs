using System.Device.Gpio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RaspberryApiTest.Configuration;
using RaspberryApiTest.Services;

namespace RaspberryApiTest.Controllers;

[Route("pump-calibration")]
public class PumpCalibrationController : ControllerBase
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    
    private readonly ILogger<PumpCalibrationController> _logger;
    private readonly IOptionsMonitor<RaspberryOptions> _raspberryOptions;
    private readonly GpioController _gpioController;
    private readonly Calibrator _calibrator;

    public PumpCalibrationController(ILogger<PumpCalibrationController> logger, 
        IOptionsMonitor<RaspberryOptions> raspberryOptions,
        GpioController gpioController,
        Calibrator calibrator)
    {
        _logger = logger;
        _raspberryOptions = raspberryOptions;
        _gpioController = gpioController;
        _calibrator = calibrator;
    }

    [HttpPost("")]
    public async Task ToggleCalibration(CancellationToken cancellationToken)
    {
        var pumpPin = _raspberryOptions.CurrentValue.PumpPin;
        
        try
        {
            await Semaphore.WaitAsync(5000, cancellationToken);
            
            TogglePump();
    
            _logger.LogInformation("Toggled pump");
        }
        finally
        {
            Semaphore.Release();
        }
    }

    private void TogglePump()
    {
        var pumpPin = _raspberryOptions.CurrentValue.PumpPin;
        var isEnabled = _gpioController.Read(pumpPin) == PinValue.Low;
        
        if (isEnabled)
        {
            _gpioController.Write(pumpPin, PinValue.High);
        }
        else
        {
            _gpioController.Write(pumpPin, PinValue.Low);
        }
    }
}