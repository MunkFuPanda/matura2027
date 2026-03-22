using DataModels;
using LinqToDB;
using LinqToDB.DataProvider.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Uebung_PA;

namespace Server
{
    class Program
    {

        public static TcpListener server = new TcpListener(System.Net.IPAddress.Any, 12345);
        public static ChatDB db = new ChatDB(new DataOptions().UseSQLite(@"Data Source=Model\chat.db"));
        public static List<Transfer<Message>> listConnectedClients = new List<Transfer<Message>>();

        public static void Main(string[] args)
        {
            Console.WriteLine("SERVER");
            server.Start();

            while (true)
            {
                TcpClient newClient = server.AcceptTcpClient();

                // db.Insert(new User { Username = "test", Password = "test" });

                Task.Run(() =>
                {
                    Transfer<Message> newTransfer = new Transfer<Message>(newClient);

                    listConnectedClients.Add(newTransfer);

                    newTransfer.OnMessageReceived += HandleMessage;
                });
            }
        }

        public static void HandleMessage(object sender, Message msg)
        {
            switch (msg.Typ)
            {
                case MessageTyp.Chat:
                    Console.WriteLine($"{msg.Username}: {msg.Content}");

                    var senderTransfer = (Transfer<Message>)sender;

                    foreach (Transfer<Message> client in listConnectedClients)
                    {
                        if (client != senderTransfer)
                        {
                            client.SendMessage(msg);
                        }
                    }
                    break;
                case MessageTyp.Login:
                    var _transfer = (Transfer<Message>)sender;
                    if (db.Users.Any(x => x.Username == msg.Username && x.Password == msg.Content))
                    {
                        Console.WriteLine($"{msg.Username} joined the chat.");
                        _transfer.SendMessage(new Message
                        {
                            Typ = MessageTyp.LoginSuccess,
                            Content = ""
                        });
                    }
                    else
                    {
                        _transfer.SendMessage(new Message
                        {
                            Typ = MessageTyp.LoginFailed,
                            Content = "Invalid username or password. Try Again."
                        });
                    }
                    break;
                case MessageTyp.Leave:
                    Console.WriteLine($"{msg.Username} left the chat.");
                    break;
            }
        }
    }
}
