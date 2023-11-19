using NotSoSmartHome.Calibration;
using NotSoSmartHome.Raspberry;
using NotSoSmartHome.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.WebHost.ConfigureKestrel((x, options) =>
{
    const int defaultPort = 5000; 
    
    var appUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
        ?.Split(";")
        .FirstOrDefault();
    if (string.IsNullOrWhiteSpace(appUrl))
    {
        options.ListenAnyIP(defaultPort);
    }
    else
    {
        var uri = new Uri(appUrl);
        options.ListenAnyIP(uri.Port);
    }
    
});

builder.Services.RegisterRaspberry(builder.Configuration);
builder.Services.AddSingleton<IGpioControllerAccessor, GpioControllerAccessor>();
builder.Services.AddSingleton<PumpCalibrationService>();

// --- Runtime middlewares ---

var app = builder.Build();

app.Services.GetRequiredService<GpioControllerAccessor>(); // Force DI initialization

app.MapControllers();

app.Run();