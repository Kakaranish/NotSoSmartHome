namespace RaspberryAgent.Config;

public class RaspberryOptions
{
    public static readonly string SectionName = "Raspberry";

    public bool UseFakeGpioController { get; set; }
    public PinConfigSection[] OpenPins { get; set; } = Array.Empty<PinConfigSection>();
}