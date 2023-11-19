using RaspberryAgent;
using RaspberryAgent.Config;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RaspberryOptions>(
    builder.Configuration.GetSection(RaspberryOptions.SectionName));

builder.Services.AddSingleton<GpioControllerFactory>();
builder.Services.AddSingleton<GpioControllerAccessor>();

// --- Runtime middlewares ---

var app = builder.Build();

app.Services.GetRequiredService<GpioControllerAccessor>(); // Force DI initialization

app.MapGet("/", () => "Hello World!");

app.Run();