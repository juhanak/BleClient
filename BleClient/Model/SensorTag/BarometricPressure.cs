using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BLESDK.Model
{
    public class BarometricPressure
        : ModelItem
    {
        public event EventHandler<BarometerValue> BarometerValueChanged;

        public BarometerValue PressureValue
        {
            get { return _BarometerValue; }
            private set
            {
                _BarometerValue = value;
                NotifyPropertyChanged();
                if (BarometerValueChanged != null)
                {
                    BarometerValueChanged(this, _BarometerValue);
                }
            }
        }

        private BarometerValue _BarometerValue;

        public BarometricPressure()
            : base()
        {
            BleCharacteristic barometerCharacteristics =
                new BleCharacteristic(Uuids.UuidDict["BarometerService"], Uuids.UuidDict["BarometerData"], true);

            BleCharacteristic barometerConfig =
                new BleCharacteristic(Uuids.UuidDict["BarometerService"], Uuids.UuidDict["BarometerConfig"]);

            BleCharacteristic barometerPeriod =
                new BleCharacteristic(Uuids.UuidDict["BarometerService"], Uuids.UuidDict["BarometerPeriod"]);

            barometerCharacteristics.BleCharacteristicChanged += OnBleBarometerCharacteristicsChanged;

            BleCharacteristics.Add("BarometerData", barometerCharacteristics);
            BleCharacteristics.Add("BarometerConfig", barometerConfig);
            BleCharacteristics.Add("BarometerPeriod", barometerPeriod);

        }

        private void OnBleBarometerCharacteristicsChanged(object sender, CharacteristicChangeArgs e)
        {

            if (e.Data != null && e.Data.Length >= 6)
            {
                BarometerValue value = new BarometerValue();

                var temperature = e.Data[2] << 16 | e.Data[1] << 8 | e.Data[0];
                var pressure = e.Data[5] << 16 | e.Data[4] << 8 | e.Data[3];
                
                value.Temperature = temperature / 100.0f;
                value.Pressure = pressure / 100.0f;

                PressureValue = value;
            }
        }

        override public async Task ConfigureAsync()
        {
            try
            {
                byte[] enable = { 0x01 };
                byte[] period = { 0x0A };
                var status = await BleCharacteristics["BarometerConfig"].Characteristic.WriteValueAsync(enable.AsBuffer(), GattWriteOption.WriteWithResponse);
                var status2 = await BleCharacteristics["BarometerPeriod"].Characteristic.WriteValueAsync(period.AsBuffer(), GattWriteOption.WriteWithResponse);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to configure Barometer");
                throw e;
            }
        }

    }
}
