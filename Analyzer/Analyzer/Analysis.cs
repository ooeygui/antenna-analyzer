using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glovebox.IoT.Devices.Converters;
using Windows.Devices.Adc;

namespace Analyzer
{
    class Analysis
    {
        public class Datapoint
        {
            public ulong frequency;
            public int transmitted;
            public int received;

            public double swr
            {
                get
                {
                    double s = transmitted / (double)received;
                    return s;
                }
            }

        };

        public List<Datapoint> datapoints = new List<Datapoint>();

        public void add(ulong freq, int tran, int rec)
        {
            datapoints.Add(new Datapoint() { frequency = freq, received = rec, transmitted = tran });
        }
    }
}
