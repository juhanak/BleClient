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
    public sealed partial class BarometerPage : Page
    {
        public SimpleAppModel AppModel
        {
            get
            {
                return SimpleAppModel.Instance;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            AppModel.Pressure.BarometerValueChanged -= Barometer_PressureChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AppModel.Pressure.BarometerValueChanged += Barometer_PressureChanged;
        }

        public BarometerPage()
        {
            this.InitializeComponent();
        }

        private void Barometer_PressureChanged(object sender, BarometerValue e)
        {
            System.Diagnostics.Debug.WriteLine("Barometric pressure: " + e.Pressure.ToString() + " Temp:" + e.Temperature.ToString());
            graphCtrl1.AddValue(e.Pressure);
            graphCtrl2.AddValue(e.Temperature);

            graphCtrl1.Title = "Pressure [hPa]: " + e.Pressure.ToString("F2");
            graphCtrl2.Title = "Temperature: " + e.Temperature.ToString("F2");
        }
    }
}
