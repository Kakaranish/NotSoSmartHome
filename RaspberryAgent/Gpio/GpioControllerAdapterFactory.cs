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
            var openPins = _raspberryOptions.Value.OpenPins.Select(p => p.Pin).ToArray();
            _logger.LogInformation("Initialized fake gpio controller adapter");
            return new FakeGpioControllerAdapter(openPins);
        }
        
        var gpioController = new GpioController(PinNumberingScheme.Logical);

        foreach (var pinConfig in _raspberryOptions.Value.OpenPins)
        {
            gpioController.OpenPin(pinConfig.Pin, pinConfig.Mode);
            
            _logger.LogInformation("Opened pin '{Pin}' in '{PinMode}' mode", 
                pinConfig.Pin, pinConfig.Mode);   
        }

        _logger.LogInformation("Initialized real gpio controller adapter");
        return new GpioControllerAdapter(gpioController);
    }
}