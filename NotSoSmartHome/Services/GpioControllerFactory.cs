using System.Device.Gpio;
using Microsoft.Extensions.Options;
using NotSoSmartHome.Configuration;

namespace NotSoSmartHome.Services;

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

        foreach (var pumpConfig in _raspberryOptions.Value.Pumps)
        {
            gpioController.OpenPin(pumpConfig.Pin, PinMode.Output);
                
            _logger.LogInformation("Opened pin '{Pin}' for pump with id '{PumpId}' ", 
                pumpConfig.Pin, pumpConfig.Id);   
        }

        return gpioController;
    }
}