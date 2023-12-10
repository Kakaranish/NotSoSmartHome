namespace NotSoSmartHome.API.Calibration;

public class Calibration
{
    private readonly Stack<CalibrationMeasurement> _measurements = new();
    
    public Guid CalibrationId { get; private set; } = Guid.NewGuid();

    public void StartMeasurement()
    {
        var lastMeasurement = _measurements.Peek();
        if (!lastMeasurement.IsCompleted)
        {
            throw new InvalidOperationException(
                "Measurement cannot be started because previous measurement is not completed");
        }

        var measurement = new CalibrationMeasurement();
        measurement.Start();
    }
    
    public void StopMeasurement()
    {
        var measurement = _measurements.Peek();
        if (measurement.IsCompleted)
        {
            throw new InvalidOperationException(
                "Measurement cannot be stopped because previous measurement is already completed");
        }
        
        measurement.Stop();
    }

    public TimeSpan GetTotalMeasurementsTimeSpan()
    {
        var result = TimeSpan.Zero;
        
        foreach (var measurement in _measurements)
        {
            if (!measurement.IsCompleted)
            {
                throw new InvalidOperationException(
                    "Cannot get total timespan because not all measurements are completed"); 
            }
            result += measurement.GetResult()!.Value;
        }

        return result;
    }
}