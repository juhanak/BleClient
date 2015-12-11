using BleClient.Internal;
using Windows.Devices.Enumeration;

namespace BleClient
{
    /// <summary>
    /// Represents a single device. Instances of this class are created and populated
    /// during the device discovery.
    /// </summary>
    public class BleDevice : NotifyPropertyChangedImpl
    {
        private string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private DeviceInformation _deviceInformation;
        public DeviceInformation DeviceInformation
        {
            get
            {
                return _deviceInformation;
            }
            private set
            {
                _deviceInformation = value;

                if (_deviceInformation != null)
                {
                    Name = _deviceInformation.Name;
                }

                NotifyPropertyChanged();
            }
        }

        public BleDevice(DeviceInformation deviceInformation)
        {
            DeviceInformation = deviceInformation;
        }
    }
}
