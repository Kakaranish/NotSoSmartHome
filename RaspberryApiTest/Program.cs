using RaspberryApiTest.Raspberry;
using RaspberryApiTest.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

builder.Services.RegisterRaspberry(builder.Configuration);
builder.Services.AddSingleton<Calibrator>();

// --- Runtime middlewares ---

var app = builder.Build();

app.MapControllers();

app.Run();