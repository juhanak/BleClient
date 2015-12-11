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
    public sealed partial class IRTemperaturePage : Page
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
            AppModel.Temperature.IrTemperatureValueChanged -= Temperature_TemperatureChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AppModel.Temperature.IrTemperatureValueChanged += Temperature_TemperatureChanged;
        }

        public IRTemperaturePage()
        {
            this.InitializeComponent();
        }

        private void Temperature_TemperatureChanged(object sender, IRTemperatureValue e)
        {
            System.Diagnostics.Debug.WriteLine("IR Temp:" + e.ObjectTemperature.ToString() + " Ambient temp:" + e.AmbientTemperature.ToString());
            graphCtrl1.AddValue(e.ObjectTemperature);
            graphCtrl2.AddValue(e.AmbientTemperature);

            graphCtrl1.Title = "Object Temperature: " + e.ObjectTemperature.ToString("F2");
            graphCtrl2.Title = "Ambient Temperature: " + e.AmbientTemperature.ToString("F2");


        }
    }
}
