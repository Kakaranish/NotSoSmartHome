namespace RaspberryApiTest.Configuration;

public class RaspberryOptions
{
    public static readonly string SectionName = "Raspberry";

    public PumpConfig[] Pumps { get; set; } = Array.Empty<PumpConfig>();
}