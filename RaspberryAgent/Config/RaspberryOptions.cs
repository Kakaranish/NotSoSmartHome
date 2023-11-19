namespace RaspberryAgent.Config;

public class RaspberryOptions
{
    public static readonly string SectionName = "Raspberry";

    public bool UseFakeGpioController { get; set; } = false;
    public PinConfig[] OpenedPins { get; set; } = Array.Empty<PinConfig>();
}