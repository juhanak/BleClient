using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BLESDK.Model
{
    public class Movement
        : ModelItem
    {
        public event EventHandler<MovementValue> MovementChanged;

        public MovementValue MovValue
        {
            get { return _movementValue; }

            private set
            {
                _movementValue = value;
                NotifyPropertyChanged();
                if(MovementChanged != null)
                {
                    MovementChanged(this, _movementValue);
                }
            }
        }
        private MovementValue _movementValue;


        public Movement()
            : base()
        {
            BleCharacteristic movementCharacteristic =
                new BleCharacteristic(Uuids.UuidDict["MovementService"], Uuids.UuidDict["MovementData"], true);

            BleCharacteristic movementConfig =
                new BleCharacteristic(Uuids.UuidDict["MovementService"], Uuids.UuidDict["MovementConfig"]);

            BleCharacteristic movementPeriod =
                new BleCharacteristic(Uuids.UuidDict["MovementService"], Uuids.UuidDict["MovementPeriod"]);

            movementCharacteristic.BleCharacteristicChanged += OnBleMovementCharacteristicChanged;

            BleCharacteristics.Add("MovementData", movementCharacteristic);
            BleCharacteristics.Add("MovementConfig", movementConfig);
            BleCharacteristics.Add("MovementPeriod", movementPeriod);
        }

        private void OnBleMovementCharacteristicChanged(object sender, CharacteristicChangeArgs e)
        {
            if (e.Data != null && e.Data.Length >= 18)
            {
                Int16 GyroXRaw = BitConverter.ToInt16(e.Data, 0);
                Int16 GyroYRaw = BitConverter.ToInt16(e.Data, 2);
                Int16 GyroZRaw = BitConverter.ToInt16(e.Data, 4);
                Int16 AccXRaw = BitConverter.ToInt16(e.Data, 6);
                Int16 AccYRaw = BitConverter.ToInt16(e.Data, 8);
                Int16 AccZRaw = BitConverter.ToInt16(e.Data, 10);
                Int16 MagXRaw = BitConverter.ToInt16(e.Data, 12);
                Int16 MagYRaw = BitConverter.ToInt16(e.Data, 14);
                Int16 MagZRaw = BitConverter.ToInt16(e.Data, 16);
                MovementValue value = new MovementValue();
                value.GyroX = GyroConvert(GyroXRaw);
                value.GyroY = GyroConvert(GyroYRaw);
                value.GyroZ = GyroConvert(GyroZRaw);
                value.AccX = AccelometerConvert(AccXRaw);
                value.AccY = AccelometerConvert(AccYRaw);
                value.AccZ = AccelometerConvert(AccZRaw);

                MovValue = value;
            }
        }

        double GyroConvert(Int16 data)
        {
            //-- calculate rotation, unit deg/s, range -250, +250
            return (data * 1.0) / (65536 / 500);
        }
        double AccelometerConvert(Int16 data)
        {
            //-- Accelometer value between -2..2
            return (data * 1.0) / (32768 / 8);
        }

        override public async Task ConfigureAsync()
        {
            try
            {
                byte[] enable = { 0x7f, 0x02 };
                byte[] period = { 0x0A };
                var status = await BleCharacteristics["MovementConfig"].Characteristic.WriteValueAsync(enable.AsBuffer(), GattWriteOption.WriteWithResponse);
                var status2 = await BleCharacteristics["MovementPeriod"].Characteristic.WriteValueAsync(period.AsBuffer(), GattWriteOption.WriteWithResponse);
            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }

    }
}
