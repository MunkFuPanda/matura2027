// See https://aka.ms/new-console-template for more information
using Client;
using NetworkLibrary;
using System.Net.Sockets;
using System.Xml.Serialization;

Console.WriteLine("Hello, Client!");

TcpClient client = new TcpClient("localhost", 12345);
Console.WriteLine("Server connected!: " + client.Client.RemoteEndPoint.ToString());

Message message = new() { TheMessage = "Grüß Gott!" };
Transfer<Message> transfer = new(client);
transfer.SendMessage(message);


Console.ReadLine();