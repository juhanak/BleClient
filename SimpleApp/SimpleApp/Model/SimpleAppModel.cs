using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLESDK.Model
{
    public class SimpleAppModel
        :AppModel
    {

        public Humidity Humidity
        {
            get;
            set;
        }

        public Movement Movement
        {
            get;
            set;
        }

        public IRTemperature Temperature
        {
            get;
            set;
        }

        public DeviceInformationService DeviceInformationService
        {
            get;
            set;
        }

        public BarometricPressure Pressure
        {
            get;
            set;
        }

        private static SimpleAppModel _instance;
        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static SimpleAppModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SimpleAppModel();
                }

                return _instance;
            }

        }

        public SimpleAppModel() :
            base()
        {
            Humidity = new Humidity();
            RegisterItem(Humidity);

            Movement = new Movement();
            RegisterItem(Movement);

            Temperature = new IRTemperature();
            RegisterItem(Temperature);

            Pressure = new BarometricPressure();
            RegisterItem(Pressure);

            DeviceInformationService = new DeviceInformationService();
            RegisterItem(DeviceInformationService);

        }

    }
}
