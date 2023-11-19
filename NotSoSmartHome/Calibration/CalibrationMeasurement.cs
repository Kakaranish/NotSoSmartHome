namespace NotSoSmartHome.Calibration;

public class CalibrationMeasurement
{
    public DateTime? StartedAtUtc { get; private set; }
    public DateTime? StoppedAtUtc { get; private set; }
    
    public bool IsCompleted => StartedAtUtc is not null && StoppedAtUtc is not null;
    
    public void Start()
    {
        if (StartedAtUtc is not null)
            throw new InvalidOperationException("Measurement is already started");
        
        StartedAtUtc = DateTime.UtcNow;
    }

    public void Stop()
    {
        if (StartedAtUtc is null)
            throw new InvalidOperationException("Measurement is not yet started");
        
        if (StoppedAtUtc is not null)
            throw new InvalidOperationException("Measurement is already stopped");

        StoppedAtUtc = DateTime.UtcNow;
    }
  
    public TimeSpan? GetResult()
    {
        return !IsCompleted
            ? null
            : StoppedAtUtc - StartedAtUtc;
    }
}