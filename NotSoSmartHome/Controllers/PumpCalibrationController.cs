using System.Device.Gpio;
using Microsoft.AspNetCore.Mvc;
using NotSoSmartHome.Services;

namespace NotSoSmartHome.Controllers;

[Route("pump-calibration")]
public class PumpCalibrationController : ControllerBase
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    
    private readonly ILogger<PumpCalibrationController> _logger;
    private readonly GpioControllerAccessor _gpioControllerAccessor;
    private readonly PumpsConfigProvider _pumpsConfigProvider;

    public PumpCalibrationController(ILogger<PumpCalibrationController> logger,
        GpioControllerAccessor gpioControllerAccessor,
        PumpsConfigProvider pumpsConfigProvider)
    {
        _logger = logger;
        _gpioControllerAccessor = gpioControllerAccessor;
        _pumpsConfigProvider = pumpsConfigProvider;
    }

    [HttpPost("")]
    public async Task<IActionResult> ToggleCalibration([FromBody] ToggleCalibrationRequestDto requestDto, 
        CancellationToken cancellationToken)
    {
        var pumpConfig = _pumpsConfigProvider.GetOrDefaultById(requestDto.PumpId);
        if (pumpConfig is null)
            return NotFound("pump not found");
        
        try
        {
            await Semaphore.WaitAsync(5000, cancellationToken);
            
            TogglePump(pumpConfig.Pin);
        }
        finally
        {
            Semaphore.Release();
        }

        return Ok();
    }

    private void TogglePump(int pumpPin)
    {
        var isEnabled = _gpioControllerAccessor.GetPinValue(pumpPin) == PinValue.Low;
        
        if (isEnabled)
        {
            _logger.LogInformation("Started calibration measurement");
            _gpioControllerAccessor.SetPinValue(pumpPin, PinValue.High);
        }
        else
        {
            _gpioControllerAccessor.SetPinValue(pumpPin, PinValue.Low);
            _logger.LogInformation("Stopped calibration measurement");
        }
    }

    public record ToggleCalibrationRequestDto(string PumpId);
}