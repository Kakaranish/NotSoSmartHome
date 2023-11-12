using System.Diagnostics;

namespace RaspberryApiTest.Services;

public class Calibrator
{
    private readonly Stopwatch _stopwatch = new();
    private readonly List<TimeSpan> _measurements = new();
    
    private readonly ILogger<Calibrator> _logger;

    public Calibrator(ILogger<Calibrator> logger)
    {
        _logger = logger;
    }

    public void ResetCalibration()
    {
        _stopwatch.Reset();
        _measurements.Clear();
    }

    public void ToggleCalibration()
    {
        if (_stopwatch.IsRunning)
        {
            _measurements.Add(TimeSpan.FromMilliseconds(_stopwatch.ElapsedMilliseconds));
            _stopwatch.Reset();
        }
        else
        {
            _stopwatch.Start();
        }
    }

    public TimeSpan GetTotalMeasurementsTimeSpan()
    {
        var result = TimeSpan.Zero;
        foreach (var measurement in _measurements)
        {
            result += measurement;
        }

        return result;
    }
}