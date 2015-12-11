using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBusClientLib;
using BLESDK.Model;
using System.IO;
using System.Runtime.Serialization.Json;

namespace BLESDK
{
    public class BleAppServiceBus
    {
        private ServiceBusClient _client;
        private Movement _movementModel;
        private static BleAppServiceBus _instance;
        private uint _skipCounter = 0;

        public static BleAppServiceBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BleAppServiceBus();
                }

                return _instance;
            }
        }

        public BleAppServiceBus()
        {
            _client = new ServiceBusClient();
        }

        public void RegisterMovementModel(Movement movementModel)
        {
            _movementModel = movementModel;
            _movementModel.MovementChanged += OnMovementChanged;
        }

        public void DeRegisterMovementModel()
        {
            _movementModel.MovementChanged -= OnMovementChanged;
            _movementModel = null;
        }

        private void OnMovementChanged(object sender, MovementValue e)
        {
            //Sends every 4th item to event hub
            if (++_skipCounter % 4 != 1)
                return;

            Task.Run(() =>
            {
                bool success = false;
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MovementValue));
                string serializedMsg = string.Empty;

                try
                {
                    serializer.WriteObject(stream, e);
                    success = true;
                }
                catch (Exception)
                {
                }
                stream.Position = 0;
                StreamReader streamReader = new StreamReader(stream);

                if (success)
                {
                    try
                    {
                        serializedMsg = streamReader.ReadToEnd();
                    }
                    catch (Exception)
                    {
                    }
                }

                if (serializedMsg != string.Empty)
                {
                    _client.SendData(serializedMsg);
                }

            });
        }
    }
}
