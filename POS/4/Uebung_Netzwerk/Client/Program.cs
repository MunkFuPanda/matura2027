using System;
using System.Net.Sockets;
using Uebung_PA;

namespace Client
{
    class Program
    {

        public static Transfer<Message> transfer;

        static TcpClient client = new TcpClient("localhost", 12345);

        static bool _loggedIn = false;
        static bool _Retry = false;

        static void Main(string[] args)
        {
            Console.WriteLine("CLIENT");
            transfer = new Transfer<Message>(client);

            transfer.OnMessageReceived += HandleMessage;

            Console.WriteLine("Verbunden!");

            string username = Login(transfer);

            while (true)
            {
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    transfer.SendMessage(new Message
                    {
                        Typ = MessageTyp.Leave,
                        Username = username,
                        Content = ""
                    });
                    client.Close();
                    break;
                }

                transfer.SendMessage(new Message
                {
                    Typ = MessageTyp.Chat,
                    Username = username,
                    Content = input
                });
            }

            
        }

        static string Login(Transfer<Message> transfer)
        {
            string username = "";
            while (!_loggedIn) {
                _Retry = false;
                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                transfer.SendMessage(new Message
                {
                    Typ = MessageTyp.Login,
                    Username = username,
                    Content = password
                });

                while (!_Retry)
                {
                    Thread.Sleep(100);
                }
            }
            return username;
        }
        static void HandleMessage(object sender, Message msg)
        {
            switch (msg.Typ)
            {
                case MessageTyp.LoginFailed:
                    _Retry = true;
                    Console.WriteLine(msg.Content);
                    break;
                case MessageTyp.LoginSuccess:
                    _Retry = true;
                    Console.WriteLine("Login Success. Nachricht eingeben: ");
                    _loggedIn = true;
                    break;
                case MessageTyp.Chat:
                    Console.WriteLine($"{msg.Username}: {msg.Content}");
                    break;
            }
        }
    }
}