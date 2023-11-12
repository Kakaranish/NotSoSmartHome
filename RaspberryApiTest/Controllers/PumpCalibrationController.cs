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
    private readonly PumpsProvider _pumpsProvider;

    public PumpCalibrationController(ILogger<PumpCalibrationController> logger, 
        IOptionsMonitor<RaspberryOptions> raspberryOptions,
        GpioController gpioController,
        Calibrator calibrator,
        PumpsProvider pumpsProvider)
    {
        _logger = logger;
        _raspberryOptions = raspberryOptions;
        _gpioController = gpioController;
        _calibrator = calibrator;
        _pumpsProvider = pumpsProvider;
    }

    [HttpPost("")]
    public async Task<IActionResult> ToggleCalibration([FromBody] ToggleCalibrationRequestDto requestDto, 
        CancellationToken cancellationToken)
    {
        var pumpConfig = _pumpsProvider.GetOrDefaultById(requestDto.PumpId);
        if (pumpConfig is null)
            return NotFound("pump not found");
        
        try
        {
            await Semaphore.WaitAsync(5000, cancellationToken);
            
            TogglePump(pumpConfig.Pin);
    
            _logger.LogInformation("Toggled pump");
        }
        finally
        {
            Semaphore.Release();
        }

        return Ok();
    }

    private void TogglePump(int pumpPin)
    {
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

    public record ToggleCalibrationRequestDto(string PumpId);
}