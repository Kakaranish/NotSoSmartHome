using System.Device.Gpio;
using Microsoft.Extensions.Options;
using RaspberryApiTest.Configuration;

namespace RaspberryApiTest.Services;

public class GpioControllerAccessor
{
    private readonly Dictionary<int, Guid> _locks = new(); // TODO: Concurrency

    private readonly ILogger<GpioControllerAccessor> _logger;
    private readonly IOptions<RaspberryOptions> _raspberryOptions;
    private readonly GpioController _gpioController;

    public GpioControllerAccessor(
        ILogger<GpioControllerAccessor> logger,
        IOptions<RaspberryOptions> raspberryOptions)
    {
        _gpioController = new GpioController(PinNumberingScheme.Logical);
        _logger = logger;
        _raspberryOptions = raspberryOptions;
        
        InitializePumpPins();
    }

    public void SetPinValue(int pinNumber, PinValue pinValue, Guid? lockId = null)
    {
        if (!_locks.TryGetValue(pinNumber, out var existingLockId))
        {
            if (lockId is not null)
            {
                _locks.TryAdd(pinNumber, lockId.Value);
            }
        }
        else
        {
            if (lockId is null)
            {
                throw new InvalidOperationException("Lock id is required to set pin value");
            }
            
            if (lockId != existingLockId)
            {
                throw new InvalidOperationException("Invalid lock id. Cannot set pin value");
            }
        }
        
        _gpioController.Write(pinNumber, pinValue);
    }

    public PinValue GetPinValue(int pinNumber)
    {
        return _gpioController.Read(pinNumber);
    }
    
    private void InitializePumpPins()
    {
        foreach (var pumpConfig in _raspberryOptions.Value.Pumps)
        {
            _gpioController.OpenPin(pumpConfig.Pin, PinMode.Output);
                
            _logger.LogInformation("Opened pin '{Pin}' for pump with id '{PumpId}' ", 
                pumpConfig.Pin, pumpConfig.Id);
        } 
    }
}