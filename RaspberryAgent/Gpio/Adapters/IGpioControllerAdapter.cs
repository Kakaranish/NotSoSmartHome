namespace RaspberryAgent.Gpio.Adapters;

public interface IGpioControllerAdapter
{
    void SetPinValue(int pinNumber, PinValue pinValue);
    PinValue? GetPinValue(int pinNumber);
    bool IsPinOpen(int pinNumber);
}