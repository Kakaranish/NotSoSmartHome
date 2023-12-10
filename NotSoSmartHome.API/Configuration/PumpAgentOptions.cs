namespace NotSoSmartHome.API.Configuration;

public class PumpAgentOptions
{
    public static readonly string SectionName = "PumpAgent";

    public string Uri { get; set; } = null!;
}