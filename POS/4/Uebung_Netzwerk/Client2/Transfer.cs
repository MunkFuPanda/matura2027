using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Uebung_PA
{
    public class Transfer<T>
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private XmlSerializer xmlSerializer = new(typeof(T));

        public EventHandler OnDisconnect;

        private EventHandler<T> _OnMessageReceived;

        public event EventHandler<T> OnMessageReceived
        {
            add { _OnMessageReceived += value; }
            remove { _OnMessageReceived -= value; }
        }


        public Transfer(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream) { AutoFlush = true };
            ThreadPool.QueueUserWorkItem(_ => Receive());
        }

        public void SendMessage(T message)
        {
            StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, message);
            _writer.WriteLine(stringWriter.ToString().ReplaceLineEndings(""));
        }

        private void Receive()
        {
            try
            {
                while (true)
                {
                    var xml = _reader.ReadLine();
                    if (xml is null) break;

                    using var sr = new StringReader(xml);
                    var message = (T)xmlSerializer.Deserialize(sr)!;
                    _OnMessageReceived?.Invoke(this, message);
                }
            }
            catch { }
            finally { OnDisconnect?.Invoke(this, EventArgs.Empty); }
        }
    }
}
