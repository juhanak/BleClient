using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BLESDK.Model
{
    public class IRTemperature 
        : ModelItem
    {
        public event EventHandler<IRTemperatureValue> IrTemperatureValueChanged;

        public IRTemperatureValue TemperatureValue
        {
            get { return _IRTemperatureValue;  }
            private set
            {
                _IRTemperatureValue = value;
                NotifyPropertyChanged();
                if(IrTemperatureValueChanged != null)
                {
                    IrTemperatureValueChanged(this, _IRTemperatureValue);
                }
            }
        }

        private IRTemperatureValue _IRTemperatureValue;
        
        public IRTemperature() 
            : base()
        {
            BleCharacteristic temperatureCharacteristics = 
                new BleCharacteristic(Uuids.UuidDict["IRTemperatureService"], Uuids.UuidDict["IRTemperatureData"], true);

            BleCharacteristic temperatureConfig = 
                new BleCharacteristic(Uuids.UuidDict["IRTemperatureService"], Uuids.UuidDict["IRTemperatureConfig"]);

            BleCharacteristic temperaturePeriod = 
                new BleCharacteristic(Uuids.UuidDict["IRTemperatureService"], Uuids.UuidDict["IRTemperaturePeriod"]);

            temperatureCharacteristics.BleCharacteristicChanged += OnBleIRTemperatureCharacteristicsChanged;

            BleCharacteristics.Add("IRTemperatureData", temperatureCharacteristics);
            BleCharacteristics.Add("IRTemperatureConfig", temperatureConfig);
            BleCharacteristics.Add("IRTemperaturePeriod", temperaturePeriod);

        }

        private void OnBleIRTemperatureCharacteristicsChanged(object sender, CharacteristicChangeArgs e)
        {
            
            if(e.Data != null && e.Data.Length >= 4)
            {
                IRTemperatureValue value = new IRTemperatureValue();

                Int16 rawObjectTemp = BitConverter.ToInt16(e.Data, 0);
                Int16 rawAmbientTemp = BitConverter.ToInt16(e.Data, 2);

                const float SCALE_LSB = 0.03125f;
                float t;
                int it;

                it = (int)((rawObjectTemp) >> 2);
                t = ((float)(it)) * SCALE_LSB;
                value.ObjectTemperature = t;

                it = (int)((rawAmbientTemp) >> 2);
                t = (float)it;
                value.AmbientTemperature = t * SCALE_LSB;

                TemperatureValue = value;
            }
        }

        override public async Task ConfigureAsync()
        {
            try
            {
                byte[] enable = { 0x01 };
                byte[] period = { 0x64 };
                var status = await BleCharacteristics["IRTemperatureConfig"].Characteristic.WriteValueAsync(enable.AsBuffer(), GattWriteOption.WriteWithResponse);
                var status2 = await BleCharacteristics["IRTemperaturePeriod"].Characteristic.WriteValueAsync(period.AsBuffer(), GattWriteOption.WriteWithResponse);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to configure IR Temperature");
                throw e;
            }
        }

    }
}
