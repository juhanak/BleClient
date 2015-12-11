using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLESDK.Model
{
    public class CharacteristicChangeArgs : EventArgs
    {
        public byte[] Data
        {
            get;
            set;
        }

    }
}
