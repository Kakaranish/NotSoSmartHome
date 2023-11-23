using System.Device.Gpio;
using Microsoft.AspNetCore.Mvc;
using NotSoSmartHome.Services;

namespace NotSoSmartHome.Controllers;

[Route("pump-calibration")]
public class PumpCalibrationController : ControllerBase
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    
    private readonly ILogger<PumpCalibrationController> _logger;
    private readonly PumpsConfigProvider _pumpsConfigProvider;

    public PumpCalibrationController(ILogger<PumpCalibrationController> logger,
        PumpsConfigProvider pumpsConfigProvider)
    {
        _logger = logger;
        _pumpsConfigProvider = pumpsConfigProvider;
    }

    [HttpPost("")]
    public async Task<IActionResult> ToggleCalibration([FromBody] ToggleCalibrationRequestDto requestDto, 
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        // var pumpConfig = _pumpsConfigProvider.GetOrDefaultById(requestDto.PumpId);
        // if (pumpConfig is null)
        //     return NotFound("pump not found");
        //
        // try
        // {
        //     await Semaphore.WaitAsync(5000, cancellationToken);
        //     
        //     TogglePump(pumpConfig.Pin);
        // }
        // finally
        // {
        //     Semaphore.Release();
        // }

        return Ok();
    }
    
    public record ToggleCalibrationRequestDto(string PumpId);
}