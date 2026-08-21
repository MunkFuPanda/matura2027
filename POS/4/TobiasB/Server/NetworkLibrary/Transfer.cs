using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Xml.Serialization;

namespace NetworkLibrary
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

        public event EventHandler<T> OnMessageReceived {
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
            _writer.WriteLine(stringWriter.ToString());
        }

        private void Receive()
        {
            try
            {
                while (true)
                {
                    String s = "";
                    String line = "";
                    while (!line.Contains("</" + typeof(T).Name + ">"))
                    {
                        line = _reader.ReadLine();
                        //Console.WriteLine("Received line: " + line);
                        s += line;
                    }

                    //Console.WriteLine(s);

                    StringReader stringReader = new StringReader(s);
                    T message = (T)xmlSerializer.Deserialize(stringReader);
                    // ? wenn null mache nichts
                    _OnMessageReceived?.Invoke(this, message);


                }
            }
            catch (Exception ex)
            {
                OnDisconnect?.Invoke(this, EventArgs.Empty);
            }
            
        }
    }
}
