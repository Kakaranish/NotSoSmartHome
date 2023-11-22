namespace RaspberryAgent.Gpio;

public enum PinValue
{
    Low,
    High
}

public static class PinValueExtensions
{
    public static LibPinValue ToLibPinValue(this PinValue pinValue)
    {
        return pinValue switch
        {
            PinValue.High => LibPinValue.High,
            PinValue.Low => LibPinValue.Low,
            _ => throw new InvalidOperationException($"'{pinValue}' cannot be converted")
        };
    }

    public static PinValue ToPinValue(this LibPinValue libPinValue)
    {
        if (libPinValue == LibPinValue.High)
            return PinValue.High;
        if (libPinValue == LibPinValue.Low)
            return PinValue.Low;

        throw new InvalidOperationException($"'{libPinValue.ToString()}' cannot be converted");
    }
}