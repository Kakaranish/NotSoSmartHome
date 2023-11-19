using System.Device.Gpio;

namespace RaspberryAgent.Config;

public class PinConfig
{
    public int Pin { get; set; }
    public PinMode Mode { get; set; }
}