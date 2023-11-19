using System.Device.Gpio;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RaspberryAgent;

public class PinValueJsonConverter : JsonConverter<PinValue>
{
    public override bool HandleNull => true;

    public override PinValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        str = str.ToLowerInvariant();
        if (str == "low")
            return PinValue.Low;
        if (str == "high")
            return PinValue.High;

        throw new JsonException("Invalid pin value");
    }

    public override void Write(Utf8JsonWriter writer, PinValue value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}