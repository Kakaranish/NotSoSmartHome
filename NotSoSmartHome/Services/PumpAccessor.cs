namespace NotSoSmartHome.Services;

public class PumpAccessor
{
    private readonly ILogger<PumpAccessor> _logger;

    public PumpAccessor(ILogger<PumpAccessor> logger)
    {
        _logger = logger;
    }
    
    
}