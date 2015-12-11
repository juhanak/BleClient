using BleClient;
using BleGame.Model;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BleGame.Controls
{
    public sealed partial class BleDeviceSelectionControl : UserControl
    {
        public ObservableCollection<BleDevice> Devices
        {
            get
            {
                return BleClientManager.Instance.Devices;
            }
        }
        public BleGameModel AppModel
        {
            get
            {
                return BleGameModel.Instance;
            }
        }

        public BleDeviceSelectionControl()
        {
            this.InitializeComponent();
        }

        public void Discover()
        {
            //BleClientManager.Instance.DiscoverServicesAsync(AppModel);
        }

        private async void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var dev = e.AddedItems[0] as BleDevice;

                if (await BleClientManager.Instance.SelectDeviceAsync(dev))
                {
                    this.Visibility = Visibility.Collapsed;
                    AppModel.EnableAllNotificationsAsync(true);
                }
            }
        }
    }
}
