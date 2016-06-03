using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;
using Windows.Devices.Enumeration;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Analyzer
{
    public class NameValueItem
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Analyzer analyzer;
        AD9850DDS ad9850;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            GpioController controller = await GpioController.GetDefaultAsync();

            GpioPin clockPin = controller.OpenPin(16);
            GpioPin dataPin = controller.OpenPin(6);
            GpioPin freq = controller.OpenPin(19);
            GpioPin reset = controller.OpenPin(5);

            ad9850 = new AD9850DDS(clockPin, freq, dataPin, reset);
            ad9850.initialize();
            ad9850.powerUp();
            ad9850.setFrequency(1000000);

            analyzer = new Analyzer();
            await analyzer.initialize(ad9850);

        }


        private void UpdateCharts(Analysis analysis)
        {
            List<NameValueItem> items = new List<NameValueItem>();
            if (analysis.datapoints == null)
            {
                return;
            }

            foreach (var datapoint in analysis.datapoints)
            {
                items.Add(new NameValueItem { Name = datapoint.frequency.ToString(), Value = datapoint.swr });
            }

            ((LineSeries)this.FreqChart.Series[0]).ItemsSource = items;
        }

        private async void Band_Click(object sender, RoutedEventArgs e)
        {
            Analysis analysis = await analyzer.analyze(Analyzer.Band.W20);

            UpdateCharts(analysis);
        }

        private void LowFre_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HighFre_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
