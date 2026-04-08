using System.Net.Sockets;
using System.Xml.Serialization;

namespace Network;

public class Transfer<T>
{
    private TcpClient _client;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;
    private XmlSerializer _serializer =  new XmlSerializer(typeof(T));
    public EventHandler<T> OnMessageReceived;
    public EventHandler OnDisconnected;
    public EventHandler<String> OnLineReceived;
    
    public Transfer(TcpClient client)
    {
        _client = client;
        _stream = _client.GetStream();
        _reader = new StreamReader(_stream);
        _writer = new StreamWriter(_stream) { AutoFlush = true };
        ThreadPool.QueueUserWorkItem(o =>  Receive());
    }

    public void Send(T data)
    {
        StringWriter stringWriter = new StringWriter();
        _serializer.Serialize(stringWriter, data);
        _writer.WriteLine(stringWriter.ToString());
    }

    private void Receive()
    {
        try
        {
            while (true)
            {
                String data = "";
                String line = "";
                while (!line.Contains("</" + typeof(T).Name + ">"))
                {
                    line =  _reader.ReadLine();
                    OnLineReceived?.Invoke(this, line);
                    data +=  line;
                }
                StringReader stringReader = new StringReader(data);
                T dataObject = (T)_serializer.Deserialize(stringReader);
                OnMessageReceived?.Invoke(this, dataObject);
            }
        }
        catch (Exception e)
        {
            OnDisconnected?.Invoke(this, EventArgs.Empty);
        }
    }
}