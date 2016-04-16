using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Analyzer
{
    public static class GPIOExtensions
    {
        public enum ShiftOutBitOrder
        {
            LeastSignificantBitFirst,
            MostSignificantBitFirst
        };


        public static void Pulse(this GpioPin pin)
        {
            pin.Write(GpioPinValue.High);
            pin.Write(GpioPinValue.Low);
        }

        public static void shiftOut(this GpioPin dataPin, GpioPin clockPin, ShiftOutBitOrder bitOrder, byte value)
        {
            for (byte i = 0; i < 8; i++)
            {
                if (bitOrder == ShiftOutBitOrder.LeastSignificantBitFirst)
                    dataPin.Write(((value & (1 << i)) == 0)? GpioPinValue.Low : GpioPinValue.High);
                else
                    dataPin.Write(((value & (1 << (7 - i))) == 0) ? GpioPinValue.Low : GpioPinValue.High);

                clockPin.Pulse();
            }
        }
    }
}
