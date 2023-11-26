using NotSoSmartHome.Configuration;
using NotSoSmartHome.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(c => {});
builder.Services.AddHttpClient();

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

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks()
    .AddCheck<PumpAgentHealthCheck>(nameof(PumpAgentHealthCheck));

builder.Services.Configure<PumpAgentOptions>(
    builder.Configuration.GetSection(PumpAgentOptions.SectionName));

// --- Runtime middlewares ---

var app = builder.Build();

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();