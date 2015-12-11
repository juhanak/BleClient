using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BLESDK.Model
{
    public class DeviceInformationService
        : ModelItem
    {
        private string _manufacturerName = "n/a";
        public string ManufacturerName
        {
            get
            {
                return _manufacturerName;
            }
            set
            {
                _manufacturerName = value;
                NotifyPropertyChanged();
            }
        }

        private string _modelNumber = "n/a";
        public string ModelNumber
        {
            get
            {
                return _modelNumber;
            }
            set
            {
                _modelNumber = value;
                NotifyPropertyChanged();
            }
        }

        private string _serialNumber = "n/a";
        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                _serialNumber = value;
                NotifyPropertyChanged();
            }
        }

        private string _hardwareRevision = "n/a";
        public string HardwareRevision
        {
            get
            {
                return _hardwareRevision;
            }
            set
            {
                _hardwareRevision = value;
                NotifyPropertyChanged();
            }
        }

        private string _firmwareRevision = "n/a";
        public string FirmwareRevision
        {
            get
            {
                return _firmwareRevision;
            }
            set
            {
                _firmwareRevision = value;
                NotifyPropertyChanged();
            }
        }

        private string _softwareRevision = "n/a";
        public string SoftwareRevision
        {
            get
            {
                return _softwareRevision;
            }
            set
            {
                _softwareRevision = value;
                NotifyPropertyChanged();
            }
        }

        private string _systemID = "n/a";
        public string SystemID
        {
            get
            {
                return _systemID;
            }
            set
            {
                _systemID = value;
                NotifyPropertyChanged();
            }
        }

        public DeviceInformationService()
            : base()
        {

            BleCharacteristic systemIdCharacteristic =
                new BleCharacteristic(Uuids.UuidDict["DeviceInfoService"], Uuids.UuidDict["SystemId"], false, true);

            BleCharacteristic modelNumberCharacteristic =
                new BleCharacteristic(Uuids.UuidDict["DeviceInfoService"], Uuids.UuidDict["ModelNumber"], false, true);

            BleCharacteristic serialNumberCharacteristic =
                new BleCharacteristic(Uuids.UuidDict["DeviceInfoService"], Uuids.UuidDict["SerialNumber"], false, true);

            BleCharacteristic firmwareRevisionCharacteristic =
                new BleCharacteristic(Uuids.UuidDict["DeviceInfoService"], Uuids.UuidDict["FirmwareRevision"], false, true);

            BleCharacteristic hardwareRevisionCharacteristic =
                new BleCharacteristic(Uuids.UuidDict["DeviceInfoService"], Uuids.UuidDict["HardwareRevision"], false, true);

            BleCharacteristic softwareRevisionCharacteristic =
                new BleCharacteristic(Uuids.UuidDict["DeviceInfoService"], Uuids.UuidDict["SoftwareRevision"], false, true);

            BleCharacteristic manufacturerNameCharacteristic =
                new BleCharacteristic(Uuids.UuidDict["DeviceInfoService"], Uuids.UuidDict["ManufacturerName"], false, true);

            BleCharacteristics.Add("SystemId", systemIdCharacteristic);
            BleCharacteristics.Add("ModelNumber", modelNumberCharacteristic);
            BleCharacteristics.Add("SerialNumber", serialNumberCharacteristic);
            BleCharacteristics.Add("FirmwareRevision", firmwareRevisionCharacteristic);
            BleCharacteristics.Add("HardwareRevision", hardwareRevisionCharacteristic);
            BleCharacteristics.Add("SoftwareRevision", softwareRevisionCharacteristic);
            BleCharacteristics.Add("ManufacturerName", manufacturerNameCharacteristic);

        }
        override public async Task ConfigureAsync()
        {
            
        }

        override public async Task<bool> ReadValuesAsync()
        {
            try
            {
                GattReadResult readResult = await BleCharacteristics["SystemId"].Characteristic.ReadValueAsync();
                byte[] data = WindowsRuntimeBufferExtensions.ToArray(readResult.Value);
                SystemID = Encoding.UTF8.GetString(data);

                readResult = await BleCharacteristics["ModelNumber"].Characteristic.ReadValueAsync();
                data = WindowsRuntimeBufferExtensions.ToArray(readResult.Value);
                ModelNumber = Encoding.UTF8.GetString(data);

                readResult = await BleCharacteristics["SerialNumber"].Characteristic.ReadValueAsync();
                data = WindowsRuntimeBufferExtensions.ToArray(readResult.Value);
                SerialNumber = Encoding.UTF8.GetString(data);

                readResult = await BleCharacteristics["FirmwareRevision"].Characteristic.ReadValueAsync();
                data = WindowsRuntimeBufferExtensions.ToArray(readResult.Value);
                FirmwareRevision = Encoding.UTF8.GetString(data);

                readResult = await BleCharacteristics["HardwareRevision"].Characteristic.ReadValueAsync();
                data = WindowsRuntimeBufferExtensions.ToArray(readResult.Value);
                HardwareRevision = Encoding.UTF8.GetString(data);

                readResult = await BleCharacteristics["SoftwareRevision"].Characteristic.ReadValueAsync();
                data = WindowsRuntimeBufferExtensions.ToArray(readResult.Value);
                SoftwareRevision = Encoding.UTF8.GetString(data);

                readResult = await BleCharacteristics["ManufacturerName"].Characteristic.ReadValueAsync();
                data = WindowsRuntimeBufferExtensions.ToArray(readResult.Value);
                ManufacturerName = Encoding.UTF8.GetString(data);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Reading failed. Reason:" + e.Message);
                return false;
            }
            return true;
        }


    }
}
