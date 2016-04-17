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

            GpioPin clockPin = controller.OpenPin(1);
            GpioPin dataPin = controller.OpenPin(2);
            GpioPin freq = controller.OpenPin(3);
            GpioPin reset = controller.OpenPin(4);

            ad9850 = new AD9850DDS(clockPin, freq, dataPin, reset);
            analyzer = new Analyzer();
            await analyzer.initialize(ad9850);
        }


        private void UpdateCharts(Analysis analysis)
        {
            List<NameValueItem> items = new List<NameValueItem>();

            foreach (var datapoint in analysis.datapoints)
            {
                items.Add(new NameValueItem { Name = datapoint.frequency.ToString(), Value = datapoint.swr });
            }

            ((LineSeries)this.FreqChart.Series[0]).ItemsSource = items;
        }

        private async void Band_Click(object sender, RoutedEventArgs e)
        {
            Analysis analysis = await analyzer.analyze(Analyzer.Band.W2);

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
