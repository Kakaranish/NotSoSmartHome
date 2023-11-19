using System.Device.Gpio;
using Microsoft.Extensions.Options;
using RaspberryAgent.Config;

namespace RaspberryAgent;

public class GpioControllerFactory
{
    private readonly ILogger<GpioControllerFactory> _logger;
    private readonly IOptions<RaspberryOptions> _raspberryOptions;

    public GpioControllerFactory(ILogger<GpioControllerFactory> logger, 
        IOptions<RaspberryOptions> raspberryOptions)
    {
        _logger = logger;
        _raspberryOptions = raspberryOptions;
    }

    public GpioController Create()
    {
        if (_raspberryOptions.Value.UseFakeGpioController)
        {
            return new FakeGpioController();
        }
        
        var gpioController = new GpioController(PinNumberingScheme.Logical);

        foreach (var pinConfig in _raspberryOptions.Value.OpenedPins)
        {
            gpioController.OpenPin(pinConfig.Pin, pinConfig.Mode);
            
            _logger.LogInformation("Opened pin '{Pin}' in '{PinMode}' mode", 
                pinConfig.Pin, pinConfig.Mode);   
        }

        return gpioController;
    }
}