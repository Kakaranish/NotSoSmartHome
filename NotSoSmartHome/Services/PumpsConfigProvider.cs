using Microsoft.Extensions.Options;
using NotSoSmartHome.Configuration;

namespace NotSoSmartHome.Services;

public class PumpsConfigProvider
{
    private readonly IOptions<RaspberryOptions> _raspberryOptions;

    public PumpsConfigProvider(IOptions<RaspberryOptions> raspberryOptions)
    {
        _raspberryOptions = raspberryOptions;
    }

    public PumpConfig? GetOrDefaultById(string pumpId)
    {
        var pumps = _raspberryOptions.Value.Pumps;

        return pumps.SingleOrDefault(p => p.Id == pumpId);
    }
}