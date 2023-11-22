using RaspberryAgent.Gpio.Adapters;

namespace RaspberryAgent.Gpio;

public enum SetPinValueResult
{
    Success,
    PinNotOpen,
    InvalidLeaseId
}

public class GpioControllerAccessor
{
    private static readonly object LeaseLock = new();
    
    private readonly Dictionary<int, Guid> _leases = new();

    private readonly IGpioControllerAdapter _gpioControllerAdapter;

    public GpioControllerAccessor(GpioControllerAdapterFactory gpioControllerAdapterFactory)
    {
        _gpioControllerAdapter = gpioControllerAdapterFactory.Create();
    }
    
    public SetPinValueResult SetPinValue(int pinNumber, PinValue pinValue, Guid? leaseId = null)
    {
        if (!_gpioControllerAdapter.IsPinOpen(pinNumber))
            return SetPinValueResult.PinNotOpen;
        
        var leaseAcquired = false;
        
        lock (LeaseLock)
        {
            if (_leases.TryGetValue(pinNumber, out var existingLeaseId))
            {
                if (leaseId is null || leaseId != existingLeaseId)
                {
                    return SetPinValueResult.InvalidLeaseId;
                }
            }
            else
            {
                if (leaseId is not null)
                {
                    _leases.TryAdd(pinNumber, leaseId.Value);
                    leaseAcquired = true;
                }
            }
        }
        
        try
        {
            _gpioControllerAdapter.SetPinValue(pinNumber, pinValue);
        }
        catch (Exception)
        {
            if (leaseAcquired)
            {
                lock (LeaseLock)
                {
                    _leases.Remove(pinNumber);
                }
            }
        }

        return SetPinValueResult.Success;
    }

    public PinValue? GetPinValue(int pinNumber)
    {
        return _gpioControllerAdapter.GetPinValue(pinNumber);
    }

    public bool BreakLease(int pinNumber)
    {
        lock (LeaseLock)
        {
            return _leases.Remove(pinNumber);
        }
    }
}