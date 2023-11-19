using System.Device.Gpio;

namespace RaspberryAgent;

public class FakeGpioController : GpioController
{
    private readonly Dictionary<int, PinValue> _pinValues = new(); 
    
    public override PinValue Read(int pinNumber)
    {
        return _pinValues.TryGetValue(pinNumber, out var pinValue)
            ? pinValue
            : PinValue.Low;
    }

    public override void Write(int pinNumber, PinValue value)
    {
        _pinValues[pinNumber] = value;
    }
}