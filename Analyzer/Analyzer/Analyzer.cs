using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glovebox.IoT.Devices.Converters;
using Windows.Devices.Adc;

namespace Analyzer
{
    class Analyzer
    {
        AD9850DDS _signalGenerator;
        AdcChannel _adcGenerated;
        AdcChannel _adcReflected;

        public enum Band
        {
            W1240 = 1240,
            W900 = 900,
            W440 = 440,
            W222 = 222,
            W2 = 2,
            W6 = 6,
            W10 = 10,
            W12 = 12,
            W15 = 15,
            W17 = 17,
            W20 = 20,
            W30 = 30,
            W40 = 40,
            W60 = 60,
            W80 = 80,
            W160 = 160
        };

        class FrequencyRange
        {
            public ulong lower;
            public ulong upper;
            public ulong step;
        }

        Dictionary<Band, FrequencyRange> BandMap = new Dictionary<Band, FrequencyRange>
        {
            { Band.W1240, new FrequencyRange {lower = 1240000000, upper = 1300000000, step = 10000000} },
            { Band.W900, new FrequencyRange {lower = 902000000, upper = 928000000, step =  10000000} },
            { Band.W440, new FrequencyRange {lower = 420000000, upper = 450000000, step = 10000000 } },
            { Band.W222, new FrequencyRange {lower = 222000000, upper = 225000000, step = 10000000 } },
            { Band.W2, new FrequencyRange {lower = 144000000, upper = 148000000, step = 10000000 } },
            { Band.W6, new FrequencyRange {lower = 50000000, upper = 54000000, step = 1000000 } },
            { Band.W10, new FrequencyRange {lower = 28000, upper = 29700, step = 100 } },
            { Band.W12, new FrequencyRange {lower = 24890, upper = 24990, step = 100 } },
            { Band.W15, new FrequencyRange {lower = 21000, upper = 21450, step = 100 } },
            { Band.W17, new FrequencyRange {lower = 18068, upper = 18168, step = 10} },
            { Band.W20, new FrequencyRange {lower = 14000, upper = 14230, step = 1} },
            { Band.W30, new FrequencyRange {lower = 10100, upper = 10150, step = 1 } },
            { Band.W40, new FrequencyRange {lower = 7000, upper = 7300, step = 10} },
            { Band.W60, new FrequencyRange {lower = 5330, upper = 5403, step = 10 } },
            { Band.W80, new FrequencyRange {lower = 3500, upper = 4000, step = 10 } },
            { Band.W160, new FrequencyRange {lower = 1800, upper = 2000, step = 10 } },
        };

        public async Task initialize()
        {
            AdcProviderManager adcManager = new AdcProviderManager();
            adcManager.Providers.Add(new MCP3002());

            IReadOnlyList<AdcController> adcControllers = await adcManager.GetControllersAsync();

            _adcGenerated = adcControllers[0].OpenChannel(0);
            _adcReflected = adcControllers[0].OpenChannel(1);
        }

        public async Task<Analysis> analyze(Band band)
        {
            ulong frequency = BandMap[band].lower;

            while (frequency <= BandMap[band].upper)
            {
                _signalGenerator.setFrequency(frequency);



                await Task.Delay(5);
            }


        }

    }
}
