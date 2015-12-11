using BLESDK;
using BLESDK.Model;
using SimpleApp.Pages;
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

namespace SimpleApp
{
    public class AppPage
    {
        public String Name
        {
            get;
            set;
        }
        public Type Type
        {
            get;
            set;
        }

    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public List<AppPage> Pages
        {
            get;
            private set;
        }
        public SimpleAppModel AppModel
        {
            get
            {
                return SimpleAppModel.Instance;
            }
        }
        public BleAppServiceBus ServiceBus
        {
            get
            {
                return BleAppServiceBus.Instance;
            }
        }


        public MainPage()
        {
            this.InitializeComponent();
            Pages = new List<AppPage>();
            Pages.Add(new AppPage() { Name = "Accelometer", Type = typeof(AccelometerPage) });
            Pages.Add(new AppPage() { Name = "Gyro", Type = typeof(GyroPage) });
            Pages.Add(new AppPage() { Name = "IR Temperature", Type = typeof(IRTemperaturePage) });
            Pages.Add(new AppPage() { Name = "Humidity", Type = typeof(HumidityPage) });
            Pages.Add(new AppPage() { Name = "Pressure", Type = typeof(BarometerPage) });
            Pages.Add(new AppPage() { Name = "Device Info", Type = typeof(DeviceInfoPage) });
            ScenarioFrame.Navigate(typeof(AccelometerPage));
        }

        override protected void OnNavigatedFrom(NavigationEventArgs e)
        {
            AppModel.EnableAllNotificationsAsync(false);

            if (ServiceBusClientLib.ServiceBusClient.IsConnectionDataSet)
            {
                ServiceBus.DeRegisterMovementModel();
            }
        }
        override protected void OnNavigatedTo(NavigationEventArgs e)
        {
            AppModel.EnableAllNotificationsAsync(true);

            //If we have set Azure Service bus settings, all changes in the model
            //will be streamed to service bus.
            if(ServiceBusClientLib.ServiceBusClient.IsConnectionDataSet)
            {
                ServiceBus.RegisterMovementModel(AppModel.Movement);
            }
            
        }

        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var page = e.AddedItems[0] as AppPage;
                ScenarioFrame.Navigate(page.Type);
            }
        }
    }
}
