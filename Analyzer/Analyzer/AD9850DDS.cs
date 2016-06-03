using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Analyzer
{
    class AD9850DDS
    {
        GpioPin _clock;
        GpioPin _frequencyUpdate;
        GpioPin _data;
        GpioPin _reset;

        const double _calibrationFrequecy = 125000000;
        byte _phase;
        ulong _deltaPhase;



        public AD9850DDS(GpioPin clock, GpioPin frequencyUpdate, GpioPin data, GpioPin reset)
        {
            _clock = clock;
            _frequencyUpdate = frequencyUpdate;
            _data = data;
            _reset = reset;
        }

        public void initialize()
        {
            _clock.SetDriveMode(GpioPinDriveMode.Output);
            _frequencyUpdate.SetDriveMode(GpioPinDriveMode.Output);
            _data.SetDriveMode(GpioPinDriveMode.Output);
            _reset.SetDriveMode(GpioPinDriveMode.Output);

            _reset.Pulse();
            _clock.Pulse();
            _frequencyUpdate.Pulse();
        }

        public void setFrequency(double frequency, int phase = 0)
        {
            _deltaPhase = (ulong)Math.Floor(frequency * 4294967296.0 / _calibrationFrequecy);

            _phase = (byte)((phase << 3) & 0xFF);

            update();
        }

        public void powerUp()
        {
            update();
        }

        public void powerDown()
        {
            _frequencyUpdate.Pulse();

            _data.shiftOut(_clock, GPIOExtensions.ShiftOutBitOrder.LeastSignificantBitFirst, 0x04);

            _frequencyUpdate.Pulse();
        }

        private void update()
        {
            ulong deltaPhase = _deltaPhase;

            for (int i = 0; i < 4; i++, deltaPhase >>= 8)
            {
                _data.shiftOut(_clock, GPIOExtensions.ShiftOutBitOrder.LeastSignificantBitFirst, (byte)(deltaPhase & 0xFF));
            }
            _data.shiftOut(_clock, GPIOExtensions.ShiftOutBitOrder.LeastSignificantBitFirst, (byte)(_phase & 0xFF));
            _frequencyUpdate.Pulse();
        }
    }
}
