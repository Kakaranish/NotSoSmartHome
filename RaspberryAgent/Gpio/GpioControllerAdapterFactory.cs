using System.Device.Gpio;
using Microsoft.Extensions.Options;
using RaspberryAgent.Config;
using RaspberryAgent.Gpio.Adapters;

namespace RaspberryAgent.Gpio;

public class GpioControllerAdapterFactory
{
    private readonly ILogger<GpioControllerAdapterFactory> _logger;
    private readonly IOptions<RaspberryOptions> _raspberryOptions;

    public GpioControllerAdapterFactory(ILogger<GpioControllerAdapterFactory> logger, 
        IOptions<RaspberryOptions> raspberryOptions)
    {
        _logger = logger;
        _raspberryOptions = raspberryOptions;
    }

    public IGpioControllerAdapter Create()
    {
        if (_raspberryOptions.Value.UseFakeGpioController)
        {
            return new FakeGpioControllerAdapter();
        }
        
        var gpioController = new GpioController(PinNumberingScheme.Logical);

        foreach (var pinConfig in _raspberryOptions.Value.OpenedPins)
        {
            gpioController.OpenPin(pinConfig.Pin, pinConfig.Mode);
            
            _logger.LogInformation("Opened pin '{Pin}' in '{PinMode}' mode", 
                pinConfig.Pin, pinConfig.Mode);   
        }

        return new GpioControllerAdapter(gpioController);
    }
}