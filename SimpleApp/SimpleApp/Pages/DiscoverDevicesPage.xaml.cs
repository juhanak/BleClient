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
using BleClient;
using BLESDK.Model;
using SimpleApp.Pages;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SimpleApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DiscoverDevicesPage : Page
    {
        public BleClientManager SdkManager
        {
            get
            {
                return BleClientManager.Instance;
            }
        }

        public SimpleAppModel AppModel
        {
            get
            {
                return SimpleAppModel.Instance;
            }
        }

        public DiscoverDevicesPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Hooks the event handlers to the events from the client.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SdkManager.DiscoverServicesAsync(AppModel);
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var dev = e.AddedItems[0] as BleDevice;
                if(await SdkManager.SelectDeviceAsync(dev))
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(MainPage));
                }

            }
        }
    }
}
