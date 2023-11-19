using NotSoSmartHome.Configuration;
using NotSoSmartHome.Services;

namespace NotSoSmartHome.Raspberry;

public static class RaspberryRegistration
{
    public static void RegisterRaspberry(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RaspberryOptions>(configuration.GetSection(RaspberryOptions.SectionName));

        services.AddScoped<PumpsConfigProvider>();

        services.AddSingleton<GpioControllerFactory>();
        services.AddSingleton<GpioControllerAccessor>();
    }
}