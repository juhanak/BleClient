using System.Collections.Generic;

using Windows.Devices.Enumeration;

namespace BLESDK.Internal
{
    class BleDeviceInformationComparer : IEqualityComparer<DeviceInformation>
    {
        public bool Equals(DeviceInformation x, DeviceInformation y)
        {
            return getDeviceGuid(x.Id) == getDeviceGuid(y.Id);
        }

        public int GetHashCode(DeviceInformation obj)
        {
            return 0; // setting to zero to force Equals comparision
        }

        private string getDeviceGuid(string deviceId)
        {
            int guidStart = deviceId.LastIndexOf("{");
            int guidLength = deviceId.LastIndexOf("}") - guidStart + 1;
            string guid = deviceId.Substring(guidStart, guidLength);
            return guid;
        }
    }
}
