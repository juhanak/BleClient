using BLESDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleGame.Model
{
    public class BleGameModel
         : AppModel
    { 

        public Movement Movement
        {
            get;
            set;
        }


        private static BleGameModel _instance;
        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static BleGameModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BleGameModel();
                }

                return _instance;
            }

        }

        public BleGameModel() :
            base()
        {
            Movement = new Movement();
            RegisterItem(Movement);
        }
    }
}
