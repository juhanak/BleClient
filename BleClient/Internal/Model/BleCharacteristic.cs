using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BLESDK.Model
{
    /// <summary>
    /// BleCharacteristic is a container for GattCharacteristic. 
    /// BleClientEngine automatically populates BleCharacteristic. 
    /// ModelItem may contain one or multiple BleCharacteristics.
    /// </summary>
    /// 
    public class BleCharacteristic
    {
        public event EventHandler<CharacteristicChangeArgs> BleCharacteristicChanged;

        public BleCharacteristic(Guid serviceGuid, Guid characteristicGuid, bool supportNotifications = false, bool supportReading = false)
        {
            ServiceGuid = serviceGuid;
            CharacteristicGuid = characteristicGuid;
            SupportNotifications = supportNotifications;
            SupportReading = supportReading;
        }

        public Guid ServiceGuid
        {
            get;
            set;
        }

        public Guid CharacteristicGuid
        {
            get;
            set;
        }

        public bool SupportNotifications
        {
            get;
            set;
        }

        public bool SupportReading
        {
            get;
            set;
        }

        public GattCharacteristic Characteristic
        {
            get;
            set;
        }

        public async Task EnableNotificationsAsync(bool enable)
        {
            if(enable && SupportNotifications)
            {
                await Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                    GattClientCharacteristicConfigurationDescriptorValue.Notify);

                Characteristic.ValueChanged += OnCharacteristicValueChanged;
            }
            else if(enable == false && SupportNotifications)
            {
                await Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                    GattClientCharacteristicConfigurationDescriptorValue.None);
            }

        }

        private async void OnCharacteristicValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            byte[] data = args.CharacteristicValue.ToArray();

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    BleCharacteristicChanged(this,new CharacteristicChangeArgs() { Data = data });
                });
        }
    }
}
