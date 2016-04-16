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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Analyzer
{
    public class NameValueItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Random _random = new Random();

        public MainPage()
        {
            this.InitializeComponent();
            UpdateCharts();
        }

        private void UpdateCharts()
        {
            List<NameValueItem> items = new List<NameValueItem>();
            items.Add(new NameValueItem { Name = "Test1", Value = _random.Next(10, 100) });
            items.Add(new NameValueItem { Name = "Test2", Value = _random.Next(10, 100) });
            items.Add(new NameValueItem { Name = "Test3", Value = _random.Next(10, 100) });
            items.Add(new NameValueItem { Name = "Test4", Value = _random.Next(10, 100) });
            items.Add(new NameValueItem { Name = "Test5", Value = _random.Next(10, 100) });

            ((LineSeries)this.FreqChart.Series[0]).ItemsSource = items;
        }

        private void Band_Click(object sender, RoutedEventArgs e)
        {

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
