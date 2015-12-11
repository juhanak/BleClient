using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BLESDK.Model
{
    public class Humidity
        : ModelItem
    {
        public event EventHandler<double> HumidityValueChanged;
        public event EventHandler<double> TemperatureValueChanged;

        public double HumidityValue
        {
            get { return _humidityValue; }

            private set
            {
                _humidityValue = value;
                NotifyPropertyChanged();
                if(HumidityValueChanged != null)
                {
                    HumidityValueChanged(this, _humidityValue);
                }
            }
        }
        private double _humidityValue;

        public double TemperatureValue
        {
            get { return _temperatureValue; }

            private set
            {
                _temperatureValue = value;
                NotifyPropertyChanged();
                if (TemperatureValueChanged != null)
                {
                    TemperatureValueChanged(this, _temperatureValue);
                }
            }
        }
        private double _temperatureValue;

        public Humidity()
            : base()
        {

            BleCharacteristic humidityCharacteristic =
                new BleCharacteristic(Uuids.UuidDict["HumidityService"], Uuids.UuidDict["HumidityData"], true);

            BleCharacteristic humidityConfig =
                new BleCharacteristic(Uuids.UuidDict["HumidityService"], Uuids.UuidDict["HumidityConfig"]);

            BleCharacteristic humidityPeriod =
                new BleCharacteristic(Uuids.UuidDict["HumidityService"], Uuids.UuidDict["HumidityPeriod"]);

            humidityCharacteristic.BleCharacteristicChanged += OnBleHumidityCharacteristicChanged;

            BleCharacteristics.Add("HumidityData", humidityCharacteristic);
            BleCharacteristics.Add("HumidityConfig", humidityConfig);
            BleCharacteristics.Add("humidityPeriod", humidityPeriod);
        }

        private void OnBleHumidityCharacteristicChanged(object sender, CharacteristicChangeArgs e)
        {
            if (e.Data != null && e.Data.Length >= 4)
            {
                Int16 rawTemp = BitConverter.ToInt16(e.Data, 0);
                Int16 rawHum = BitConverter.ToInt16(e.Data, 2);

                TemperatureValue = ((double)(Int16)rawTemp / 65536) * 165 - 40;
                HumidityValue = ((double)rawHum / 65536) * 100;
            }
        }

        override public async Task ConfigureAsync()
        {
            try { 
                byte[] enable = { 0x01 };
                byte[] period = { 0x64 };
                var status = await BleCharacteristics["HumidityConfig"].Characteristic.WriteValueAsync(enable.AsBuffer(), GattWriteOption.WriteWithResponse);
                var status2 = await BleCharacteristics["humidityPeriod"].Characteristic.WriteValueAsync(period.AsBuffer(), GattWriteOption.WriteWithResponse);
            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to configure Humidity");
                throw e;
            }
        }

    }
}
