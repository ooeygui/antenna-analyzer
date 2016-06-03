using System;
using System.Threading.Tasks;
using Windows.Devices.Adc.Provider;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using System.ComponentModel;
using System.Linq;

namespace Analyzer
{

    public sealed class MCP3002 : IAdcControllerProvider, IDisposable {

        #region Constants
        private const byte SINGLE_ENDED_MODE = 0x08;
        private const byte PSEUDO_DIFFERENTIAL_MODE = 0x00;
        private const byte ChipMode = SINGLE_ENDED_MODE;
        #endregion // Constants

        #region Member Variables
        static private bool isInitialized;
        static private SpiDevice spiDevice;            // The SPI device the display is connected to
        #endregion // Member Variables



        public string ControllerName { get; set; } = "SPI0";

        public int ChipSelectLine { get; set; } = 0;

        public int ChannelCount => 2;

        public int MaxValue => 1023;

        public int MinValue => 0;

        public int ResolutionInBits => 10;

        public ProviderAdcChannelMode ChannelMode { get; set; } = ProviderAdcChannelMode.SingleEnded;



        #region Internal Methods
        public async Task EnsureInitializedAsync()
        {
            if (isInitialized)
            {
                return;
            }

            try
            {
                var settings = new SpiConnectionSettings(ChipSelectLine);
                settings.ClockFrequency = 1350000; // 500000;// 10000000;
                settings.Mode = SpiMode.Mode0; //Mode3;

                string spiAqs = SpiDevice.GetDeviceSelector();
                var deviceInfo = await DeviceInformation.FindAllAsync(spiAqs);
                spiDevice = await SpiDevice.FromIdAsync(deviceInfo[0].Id, settings);

                isInitialized = true;
            }
            /* If initialization fails, display the exception and stop running */
            catch (Exception ex)
            {
                throw new Exception("SPI Initialization Failed", ex);
            }

        }
        #endregion // Internal Methods



        public bool IsChannelModeSupported(ProviderAdcChannelMode channelMode) {
            return channelMode == ProviderAdcChannelMode.SingleEnded ? true : false;
        }


        public int ReadValue(int channelNumber)
        {
            byte command = (byte)channelNumber;
            if (ChannelMode == ProviderAdcChannelMode.SingleEnded)
            {
                command |= SINGLE_ENDED_MODE;
            }
            command <<= 4;

            byte[] commandBuf = new byte[] { 0x01, command, 0x00 };

            byte[] readBuf = new byte[] { 0x00, 0x00, 0x00 };

            spiDevice.TransferFullDuplex(commandBuf, readBuf);

            int sample = readBuf[2] + ((readBuf[1] & 0x03) << 8);

            return sample;
        }

        public void AcquireChannel(int channel) {
            if ((channel < 0) || (channel > ChannelCount)) throw new ArgumentOutOfRangeException("channel");
        }

        public void ReleaseChannel(int channel) {
            if ((channel < 0) || (channel > ChannelCount)) throw new ArgumentOutOfRangeException("channel");
        }

        public void Dispose() {
            spiDevice?.Dispose();
            spiDevice = null;
            isInitialized = false;
        }
    }
}
