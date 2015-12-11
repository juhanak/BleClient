using Amqp;
using Amqp.Framing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusClientLib
{
    public class ServiceBusClient
    {
        static string _servicebusNamespace = null;
        static string _eventHubName = null;
        static string _saPolicyName = null;
        static string _saKey = null;
        static string _partitionkey = null;

        Amqp.Address _address;
        Amqp.Connection _connection = null;
        Amqp.Session _session;
        Amqp.SenderLink _senderlink;

        static public bool IsConnectionDataSet
        {
            get
            {
                if (_servicebusNamespace != null &&
                    _servicebusNamespace != string.Empty &&
                    _eventHubName != null &&
                    _eventHubName != string.Empty &&
                    _saPolicyName != null &&
                    _saPolicyName != string.Empty &&
                    _saKey != null &&
                    _saKey != string.Empty &&
                    _partitionkey != null &&
                    _partitionkey != string.Empty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        void Connect()
        {
            _address = new Amqp.Address(
                string.Format("{0}.servicebus.windows.net", _servicebusNamespace),
                5671, _saPolicyName, _saKey);

           _connection = new Amqp.Connection(_address);
           _session = new Amqp.Session(_connection);
           _senderlink = new Amqp.SenderLink(_session,
                string.Format("send-link:{0}", _saPolicyName), _eventHubName);
        }

        public void SendData(string data)
        {
            if(_connection == null)
            {
                Connect();
            }

            Amqp.Message message = new Amqp.Message()
            {
                BodySection = new Amqp.Framing.Data()
                {
                    Binary = System.Text.Encoding.UTF8.GetBytes(data)
                }
            };

            ManualResetEvent acked = new ManualResetEvent(false);
            message.MessageAnnotations = new Amqp.Framing.MessageAnnotations();
            message.MessageAnnotations[new Amqp.Types.Symbol("x-opt-partition-key")] =
               string.Format("pk:", _partitionkey);

            try { 
            _senderlink.Send(message);
            } catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Sending failed");
                _connection = null;
            }
        }
    }
}
