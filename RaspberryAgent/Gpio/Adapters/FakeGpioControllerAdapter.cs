namespace RaspberryAgent.Gpio.Adapters;

public class FakeGpioControllerAdapter : IGpioControllerAdapter
{
    private readonly int[] _openPins;
    private readonly Dictionary<int, PinValue> _pinValues = new();

    public FakeGpioControllerAdapter(int[] openPins)
    {
        _openPins = openPins;
    }

    public void SetPinValue(int pinNumber, PinValue pinValue)
    {
        _pinValues[pinNumber] = pinValue;
    }

    public PinValue? GetPinValue(int pinNumber)
    {
        return _pinValues.TryGetValue(pinNumber, out var pinValue)
            ? pinValue
            : PinValue.Low;
    }

    public bool IsPinOpen(int pinNumber)
    {
        return _openPins.Contains(pinNumber);
    }

    public bool IsGpioAccessible()
    {
        return true;
    }
}