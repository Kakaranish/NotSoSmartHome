using System.Collections.Concurrent;
using RaspberryApiTest.Services;
using RaspberryApiTest.Types;

namespace RaspberryApiTest.Calibration;

public class PumpCalibrationService
{
    private readonly ConcurrentDictionary<PumpId, Calibration> _calibrations = new();

    private readonly ILogger<PumpCalibrationService> _logger;
    private readonly IGpioControllerAccessor _gpioControllerAccessor;
    
    public PumpCalibrationService(
        ILogger<PumpCalibrationService> logger,
        IGpioControllerAccessor gpioControllerAccessor)
    {
        _logger = logger;
        _gpioControllerAccessor = gpioControllerAccessor;
    }

    public void StartCalibrationMeasurement(PumpId pumpId)
    {
        _logger.LogInformation("Starting calibration for pump '{PumpId}'", pumpId.Value);

        var calibration = _calibrations.GetOrAdd(pumpId, _ => new Calibration());
    }

    public void StopCalibrationMeasurement(PumpId pumpId)
    {
        _logger.LogInformation("Stopped calibration for pump '{PumpId}'", pumpId.Value);
    }

    public void CompleteCalibration(PumpId pumpId)
    {
        _logger.LogInformation("Calibration completed for pump '{PumpId}'", pumpId.Value);
    }
}