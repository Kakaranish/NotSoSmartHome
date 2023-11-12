using System.Device.Gpio;
using Microsoft.Extensions.Options;
using RaspberryApiTest.Configuration;

namespace RaspberryApiTest.Raspberry;

public static class RaspberryRegistration
{
    public static void RegisterRaspberry(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RaspberryOptions>(configuration.GetSection(RaspberryOptions.SectionName));

        services.AddSingleton<GpioController>(sp =>
        {
            var raspberryOptions = sp.GetRequiredService<IOptionsMonitor<RaspberryOptions>>();
            var gpioController = new GpioController(PinNumberingScheme.Logical);
            gpioController.OpenPin(raspberryOptions.CurrentValue.PumpPin, PinMode.Output);
            
            return gpioController;
        });
    }
}