using RaspberryAgent;
using RaspberryAgent.Config;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new PinValueJsonConverter());
    });

builder.Services.Configure<RaspberryOptions>(
    builder.Configuration.GetSection(RaspberryOptions.SectionName));

builder.Services.AddSingleton<GpioControllerFactory>();
builder.Services.AddSingleton<GpioControllerAccessor>();

// --- Runtime middlewares ---

var app = builder.Build();

app.Services.GetRequiredService<GpioControllerAccessor>(); // Force DI initialization

app.MapControllers();

app.Run();