// See https://aka.ms/new-console-template for more information
using NetworkLibrary;
using Server;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;

Console.WriteLine("Hello, Server!");

TcpListener server = new TcpListener(IPAddress.Any, 12345);
server.Start();

// bleibt stehen bis ein client connected, daher Thread, kein GUI Code!!!
TcpClient client = server.AcceptTcpClient();

Console.WriteLine("Client connected!: " + client.Client.RemoteEndPoint.ToString());

//byte[] buffer = new byte[1024];

//int read = networkStream.Read(buffer);

//String message = System.Text.Encoding.UTF8.GetString(buffer, 0, read);


// String message = reader.ReadLine();

// Will gesamtes XML haben, ReadtoEnd, Networkstream hat kein Ende

//Message message = (Message)serializer.Deserialize(reader);


Transfer<Message> transfer = new(client);
transfer.OnMessageReceived += (sender, e) =>
{
    Console.WriteLine("Received message: " + e.TheMessage);
};

transfer.OnDisconnect += (sender, e) =>
{
    Console.WriteLine("Client disconnected");
};


Console.ReadLine();