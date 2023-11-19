using System.Device.Gpio;

namespace NotSoSmartHome.Services;

public class FakeGpioController : GpioController
{
    private readonly Dictionary<int, PinValue> _pinValues = new(); 
    
    public override PinValue Read(int pinNumber)
    {
        return _pinValues.TryGetValue(pinNumber, out var pinValue)
            ? pinValue
            : PinValue.Low;
    }

    public override void Write(int pinNumber, PinValue value)
    {
        _pinValues[pinNumber] = value;
    }
}

public interface IGpioControllerAccessor
{
    void SetPinValue(int pinNumber, PinValue pinValue, Guid? leaseId = null);
    PinValue GetPinValue(int pinNumber);
    bool BreakLease(int pinNumber);
}

public class GpioControllerAccessor : IGpioControllerAccessor
{
    private static readonly object LeaseLock = new();
    
    private readonly Dictionary<int, Guid> _leases = new();

    private readonly GpioController _gpioController;

    public GpioControllerAccessor(GpioControllerFactory gpioControllerFactory)
    {
        _gpioController = gpioControllerFactory.Create();
    }
    
    public void SetPinValue(int pinNumber, PinValue pinValue, Guid? leaseId = null)
    {
        var leaseAcquired = false;
        
        lock (LeaseLock)
        {
            if (_leases.TryGetValue(pinNumber, out var existingLeaseId))
            {
                if (leaseId is null)
                {
                    throw new InvalidOperationException($"Pin is already leased. Provide '{nameof(leaseId)}' to change its value");
                }
            
                if (leaseId != existingLeaseId)
                {
                    throw new InvalidOperationException($"Invalid '{nameof(leaseId)}'");
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
            _gpioController.Write(pinNumber, pinValue);
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
    }

    public PinValue GetPinValue(int pinNumber)
    {
        return _gpioController.Read(pinNumber);
    }

    public bool BreakLease(int pinNumber)
    {
        lock (LeaseLock)
        {
            return _leases.Remove(pinNumber);
        }
    }
}