using System.Device.Gpio;

namespace RaspberryAgent.Config;

public class PinConfigSection
{
    public int Pin { get; set; }
    public PinMode Mode { get; set; }
}