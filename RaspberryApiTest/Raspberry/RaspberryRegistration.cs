using System.Device.Gpio;
using Microsoft.Extensions.Options;
using RaspberryApiTest.Configuration;
using RaspberryApiTest.Services;

namespace RaspberryApiTest.Raspberry;

public static class RaspberryRegistration
{
    public static void RegisterRaspberry(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RaspberryOptions>(configuration.GetSection(RaspberryOptions.SectionName));

        services.AddScoped<PumpsProvider>();
        
        services.AddSingleton<GpioController>(sp =>
        {
            var raspberryOptions = sp.GetRequiredService<IOptions<RaspberryOptions>>();
            var logger = sp.GetRequiredService<ILogger<Program>>();
            
            var gpioController = new GpioController(PinNumberingScheme.Logical);

            foreach (var pumpConfig in raspberryOptions.Value.Pumps)
            {
                gpioController.OpenPin(pumpConfig.Pin, PinMode.Output);
                
                logger.LogInformation("Opened pin '{Pin}' for pump with id '{PumpId}' ", 
                    pumpConfig.Pin, pumpConfig.Id);
            }
            
            return gpioController;
        });
    }
}