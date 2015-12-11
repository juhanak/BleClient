using BleClient.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLESDK.Model
{
    /// <summary>
    /// Any entity that needs access to Ble characters should be inherited from ModelItem ( see DeviceInformationService.cs).
    /// ModelItem can contain characters from one or many Services.
    /// </summary>
    public class ModelItem :
        NotifyPropertyChangedImpl
    {
        public ModelItem()
        {
            BleCharacteristics = new Dictionary<string, BleCharacteristic>();
        }

        public IDictionary<string,BleCharacteristic> BleCharacteristics
        {
            get;
            protected set;
        }

        virtual public async Task ConfigureAsync()
        {
            throw new Exception("not implemented");
        }

        virtual public async Task<bool> ReadValuesAsync()
        {
            return true;
        }

    }
}
