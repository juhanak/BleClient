using BleClient.Internal;
using BLESDK.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BleClient
{
    public class BleClientManager
    {
        private BleClientEngine _engine;

        /// <summary>
        /// List of devices
        /// </summary>
        public ObservableCollection<BleDevice> Devices
        {
            get
            {
                return _engine.Devices;
            }
        }

        /// <summary>
        /// The singleton instance
        /// </summary>
        public static BleClientManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BleClientManager();
                }

                return _instance;
            }
        }
        private static BleClientManager _instance;

        /// <summary>
        /// Constructor
        /// </summary>
        public BleClientManager()
        {
            _engine = new BleClientEngine();
        }

        /// <summary>
        /// Discovers all devices that have the same services with the model
        /// </summary>
        /// <param name="appModel"></param>
        public async Task DiscoverServicesAsync(AppModel appModel)
        {
            await _engine.DiscoverDevicesAsync(appModel);
        }

        /// <summary>
        /// Selects the device
        /// </summary>
        /// <param name="selectedDevice"></param>
        public async Task<bool> SelectDeviceAsync(BleDevice selectedDevice)
        {
            return await _engine.SelectDeviceAsync(selectedDevice);
        }
    }
}
