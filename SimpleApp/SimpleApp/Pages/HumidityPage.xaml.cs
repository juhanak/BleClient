using BLESDK.Model;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SimpleApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HumidityPage : Page
    {
        public SimpleAppModel AppModel
        {
            get
            {
                return SimpleAppModel.Instance;
            }
        }

        public HumidityPage()
        {
            this.InitializeComponent();
        }

        override protected void OnNavigatedFrom(NavigationEventArgs e)
        {
            AppModel.Humidity.HumidityValueChanged -= OnHumidityValueChanged;
            AppModel.Humidity.TemperatureValueChanged -= OnTemperatureValueChanged;
        }

        override protected void OnNavigatedTo(NavigationEventArgs e)
        {
            AppModel.Humidity.HumidityValueChanged += OnHumidityValueChanged;
            AppModel.Humidity.TemperatureValueChanged += OnTemperatureValueChanged;
        }

        private void OnTemperatureValueChanged(object sender, double e)
        {
            //System.Diagnostics.Debug.WriteLine("Temperature:" + e.ToString());
            graphCtrl2.Title = "Temperature: " + e.ToString("F2");
            graphCtrl2.AddValue(e);
        }

        private void OnHumidityValueChanged(object sender, double e)
        {
            //System.Diagnostics.Debug.WriteLine("Humidity:" + e.ToString());
            graphCtrl1.Title = "Humidity: " + e.ToString("F2");
            graphCtrl1.AddValue(e);
        }

    }
}
