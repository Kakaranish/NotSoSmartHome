using System.Text.Json.Serialization;
using RaspberryAgent.Config;
using RaspberryAgent.Gpio;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.Configure<RaspberryOptions>(
    builder.Configuration.GetSection(RaspberryOptions.SectionName));

builder.Services.AddSingleton<GpioControllerAdapterFactory>();
builder.Services.AddSingleton<GpioControllerAccessor>();

// --- Runtime middlewares ---

var app = builder.Build();

app.Services.GetRequiredService<GpioControllerAccessor>(); // Force DI initialization

app.MapControllers();

app.Run();