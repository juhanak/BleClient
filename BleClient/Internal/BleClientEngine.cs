using BLESDK.Model;
using BLESDK.Internal;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace BleClient.Internal
{
    public class BleClientEngine
    {
        GattDeviceService _deviceInformationService;
        Hashtable _services = new Hashtable();

        /// <summary>
        /// List of discovered Devices.
        /// </summary>
        public ObservableCollection<BleDevice> Devices
        {
            get;
            private set;
        }

        public BleDevice SelectedDevice
        {
            get;
            private set;
        }

        public AppModel Model
        {
            get;
            private set;
        }

        /// <summary>
        /// Indicates whether the GATT device services have been acquired or not.
        /// </summary>
        public bool IsInitialized
        {
            get;
            private set;
        }

        public BleClientEngine()
        {
            Devices = new ObservableCollection<BleDevice>();
        }


        /// <summary>
        /// Discovers all Bluetooth Devices that have support for services in app model
        /// </summary>
        public async Task DiscoverDevicesAsync(AppModel appModel)
        {
            Model = appModel;

            Devices.Clear();

            // Find all unique service GUIDs
            HashSet<Guid> serviceGuids = new HashSet<Guid>();

            foreach (var modelItem in Model.Items)
            {
                foreach (var chModel in modelItem.BleCharacteristics)
                {
                    serviceGuids.Add(chModel.Value.ServiceGuid);
                }
            }

            // Find all devices that have support for individual services
            List<List<DeviceInformation>> devices = new List<List<DeviceInformation>>();

            foreach (Guid guid in serviceGuids)
            {
                DeviceInformationCollection deviceList = await DeviceInformation.FindAllAsync(
                GattDeviceService.GetDeviceSelectorFromUuid(guid));
                devices.Add(deviceList.ToList<DeviceInformation>());
            }

            if (devices.Count > 0)
            {
                // Intersect to resolve devices that support all the requested services
                var commonDevices = devices.Aggregate((previousList, nextList) => previousList.Intersect(nextList, new BleDeviceInformationComparer()).ToList());

                if (commonDevices.Count > 0)
                {
                    foreach (var deviceInformation in commonDevices)
                    {
                        BleDevice device = new BleDevice(deviceInformation);
                        Devices.Add(device);
                    }
                }
            }

        }


        /// <summary>
        /// Initializes (acquires) the GATT services.
        /// </summary>
        /// <param name="device">The headset device whose services to initialize.</param>
        /// <returns>True, if successful. False otherwise.</returns>
        public async Task<bool> SelectDeviceAsync(BleDevice device)
        {
            IsInitialized = false;

            if (_deviceInformationService != null)
            {
                _deviceInformationService.Device.ConnectionStatusChanged -= OnConnectionStatusChanged;
            }

            SelectedDevice = device;
            _deviceInformationService = await GattDeviceService.FromIdAsync(device.DeviceInformation.Id);

            
            if (_deviceInformationService != null)
            {

                foreach (var item in _deviceInformationService.Device.GattServices)
                {
                    _services.Add(item.Uuid, item);
                }

                _deviceInformationService.Device.ConnectionStatusChanged += OnConnectionStatusChanged;

                IsInitialized = await PopulateModelAsync();

            }

            return IsInitialized;
        }

        private void OnConnectionStatusChanged(BluetoothLEDevice sender, object args)
        {
            if(sender.ConnectionStatus == BluetoothConnectionStatus.Connected)
            {
                System.Diagnostics.Debug.WriteLine("Connected");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Disconnected");
            }
            
        }

        private async Task<bool> PopulateModelAsync()
        {
            try
            {
                foreach (var modelItem in Model.Items)
                {
                    foreach (var chModel in modelItem.BleCharacteristics)
                    {
                        GattDeviceService gattService = _services[chModel.Value.ServiceGuid] as GattDeviceService;
                        var character = gattService.GetCharacteristics(chModel.Value.CharacteristicGuid)[0];
                        chModel.Value.Characteristic = character;
                    }
                }
            } 
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to populate model.");
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
            try
            {
                foreach (var modelItem in Model.Items)
                {
                    await modelItem.ConfigureAsync();
                }
            } 
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to configure models.");
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}
