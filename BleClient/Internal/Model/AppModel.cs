using BleClient.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLESDK.Model
{
    /// <summary>
    /// AppModel represents Ble device data for the application. AppModel contains ModelItems which can 
    /// be used to expose different Ble services to the application. Each application should implement own Appmodel. 
    /// </summary>
    /// 
    public class AppModel :
        NotifyPropertyChangedImpl
    {
        public IList<ModelItem> Items
        {
            get;
            private set;
        }

        public AppModel()
        {
            Items = new List<ModelItem>();
        }

        public async Task EnableAllNotificationsAsync(bool enable)
        {
            foreach (var item in Items)
            {
                foreach (var character in item.BleCharacteristics)
                {
                    await character.Value.EnableNotificationsAsync(enable);
                }
             }
        }

        protected void RegisterItem(ModelItem item)
        {
            Items.Add(item);
        }
    }
}

