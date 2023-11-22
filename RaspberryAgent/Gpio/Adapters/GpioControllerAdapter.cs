using System.Device.Gpio;

namespace RaspberryAgent.Gpio.Adapters;

public class GpioControllerAdapter : IGpioControllerAdapter
{
    private readonly GpioController _gpioController;

    public GpioControllerAdapter(GpioController gpioController)
    {
        _gpioController = gpioController;
    }

    public void SetPinValue(int pinNumber, PinValue pinValue)
    {
        _gpioController.Write(pinNumber, pinValue.ToLibPinValue());
    }

    public PinValue? GetPinValue(int pinNumber)
    {
        if (!_gpioController.IsPinOpen(pinNumber)) return null;
        
        var libPinValue = _gpioController.Read(pinNumber);
        return libPinValue.ToPinValue();
    }

    public bool IsPinOpen(int pinNumber)
    {
        return _gpioController.IsPinOpen(pinNumber);
    }
}