using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLESDK.Model
{
    
    class Uuids
    {
        // Base UUID for TI Sensor Tag
        private static string TIBaseUUID = "f000{0,4:X}-0451-4000-b000-000000000000";
        private static string BLEBaseUUID = "0000{0,4:X}-0000-1000-8000-00805f9b34fb";

        public static readonly Dictionary<string, Guid> UuidDict
        = new Dictionary<string, Guid>
        {
            //Device Info Service
            {"DeviceInfoService", CreateGuidfromShortUUID( 0x180A, BLEBaseUUID ) },
            {"SystemId" , CreateGuidfromShortUUID( 0x2A23, BLEBaseUUID ) },
            {"ModelNumber" , CreateGuidfromShortUUID( 0x2A24, BLEBaseUUID ) },
            {"SerialNumber" , CreateGuidfromShortUUID( 0x2A25, BLEBaseUUID ) },
            {"FirmwareRevision" , CreateGuidfromShortUUID( 0x2A26, BLEBaseUUID ) },
            {"HardwareRevision" , CreateGuidfromShortUUID( 0x2A27, BLEBaseUUID ) },
            {"SoftwareRevision" , CreateGuidfromShortUUID( 0x2A28, BLEBaseUUID ) },
            {"ManufacturerName" , CreateGuidfromShortUUID( 0x2A29, BLEBaseUUID ) },

            //Humidity Sensor
            {"HumidityService" , CreateGuidfromShortUUID(0xAA20, TIBaseUUID ) },
            {"HumidityData" , CreateGuidfromShortUUID( 0xAA21, TIBaseUUID ) },
            {"HumidityConfig" , CreateGuidfromShortUUID( 0xAA22, TIBaseUUID ) },
            {"HumidityPeriod" , CreateGuidfromShortUUID( 0xAA23, TIBaseUUID ) },

            //Movement Sensor
            {"MovementService" , CreateGuidfromShortUUID(0x0AA80, TIBaseUUID ) },
            {"MovementData" , CreateGuidfromShortUUID( 0xAA81, TIBaseUUID ) },
            {"MovementConfig" , CreateGuidfromShortUUID( 0xAA82, TIBaseUUID ) },
            {"MovementPeriod" , CreateGuidfromShortUUID( 0xAA83, TIBaseUUID ) },
            
            //IR Sensor
            {"IRTemperatureService" , CreateGuidfromShortUUID( 0xAA00, TIBaseUUID ) },
            {"IRTemperatureData" , CreateGuidfromShortUUID( 0xAA01, TIBaseUUID ) },
            {"IRTemperatureConfig" , CreateGuidfromShortUUID( 0xAA02, TIBaseUUID ) },
            {"IRTemperaturePeriod" , CreateGuidfromShortUUID( 0xAA03, TIBaseUUID ) },

            //Barometer sensor
            {"BarometerService" , CreateGuidfromShortUUID( 0xAA40, TIBaseUUID ) },
            {"BarometerData" , CreateGuidfromShortUUID( 0xAA41, TIBaseUUID ) },
            {"BarometerConfig" , CreateGuidfromShortUUID( 0xAA42, TIBaseUUID ) },
            {"BarometerPeriod" , CreateGuidfromShortUUID( 0xAA44, TIBaseUUID ) }
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uuid16Bit"></param>
        /// <returns></returns>
        public static Guid CreateGuidfromShortUUID(int uuid16Bit, string baseUUID)
        {
            string uuidString = string.Format(baseUUID, uuid16Bit);

            if (uuid16Bit >= 0 && uuid16Bit < 0xFFFF)
            {
                return Guid.Parse(uuidString);
            }

            return Guid.Empty;
        }

    }



    

}
