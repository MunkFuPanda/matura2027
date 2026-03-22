using DataModels;
using LinqToDB;
using NetworkLibrary;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using WPF_Chat_Client;
using Message = WPF_Chat_Client.Message;

namespace WPF_Chat_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        TcpListener listener;
        List<Transfer<Message>> clients = new();
        List<Transfer<Message>> auth_clients = new();
        Dictionary<Transfer<Message>, User> clientUserMap = new();

        List<User> users = new();
        List<DataModels.Message> messages = new();
        List<Room> rooms = new();
        List<UserRoom> userRooms = new();

        public MainWindow()
        {
            InitializeComponent();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345);
            listener.Start();
            Log("Server started on port 12345");
            ThreadPool.QueueUserWorkItem(_ => AcceptClients());

            //db connection
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db\\chat.db");

            var options = new DataOptions()
                .UseSQLite($"Data Source={path}");

            //db abfrage
            using (var db = new ChatDB(options))
            {
                var rooms = db.Rooms.ToList();

                foreach (var r in rooms)
                {
                    Log(r.Name);
                }
            }

            using (var db = new ChatDB(options))
            {
                users = db.Users.ToList();
                rooms = db.Rooms.ToList();
                messages = db.Messages.ToList();
                userRooms = db.UserRooms.ToList();
            }
        }

        private string GetUserName(long userId)
        {
            var user = users.FirstOrDefault(u => u.UserID == userId);
            return user?.Name ?? $"User {userId}";
        }

        private void AcceptClients()
        {
            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    var transfer = new Transfer<Message>(client);
                    clients.Add(transfer);

                    transfer.OnMessageReceived += Transfer_OnMessageReceived;
                    transfer.OnDisconnect += (s, e) =>
                    {
                        clients.Remove(transfer);
                        auth_clients.Remove(transfer);
                        clientUserMap.Remove(transfer);
                        Log("Client disconnected");
                    };

                    Log("Client connected");
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        private void Transfer_OnMessageReceived(object? sender, Message e)
        {
            Log($"[{(MessageType)e.Type}] {e.Message_Text}");

            if (e.Type == 0) // Authenticate
            {
                String[] parts = e.Message_Text.Split(':');
                foreach (var user in users)
                {
                    if (user.Name == parts[0] && user.Passwort == parts[1])
                    {
                        var senderTransfer = (Transfer<Message>)sender!;
                        auth_clients.Add(senderTransfer);
                        clientUserMap[senderTransfer] = user;
                        Log("Client authenticated");

                        HashSet<int> roomid = new HashSet<int>();

                        foreach (var roomUser in userRooms) { 
                            if (roomUser.UserID == user.UserID)
                            {
                                roomid.Add((int)roomUser.RoomID!);
                            }
                        }

                        senderTransfer.SendMessage(new Message
                        {
                            Type = 2,
                            Message_Text = string.Join(":",
                                rooms
                                    .Where(r => roomid.Contains((int)r.RoomID))
                                    .Select(r => $"{r.RoomID};{r.Name}")
                            )
                        });

                        List<DataModels.Message> messagesToSend = new List<DataModels.Message>();
                        foreach (var message in messages)
                        {
                            if (roomid.Contains((int)message.ToID))
                            {
                                messagesToSend.Add(message);
                            }
                        }

                        foreach (var message in messagesToSend)
                        {
                            senderTransfer.SendMessage(new Message
                            {
                                Type = 1,
                                Message_Text = message.Text,
                                From_UserId = (int)message.FromID,
                                From_UserName = GetUserName(message.FromID),
                                To_RommId = (int)message.ToID,
                                date = (DateTime)message.Date!
                            });
                        }

                            return;
                    }
                }
                Log("Wrong User - not authenticated");
                return;
            }
            else if (e.Type == 1)
            {
                var senderTransfer = (Transfer<Message>)sender!;

                // Prüfen ob der Client authentifiziert ist
                if (!clientUserMap.TryGetValue(senderTransfer, out var senderUser))
                {
                    Log("Unauthenticated client tried to send a message");
                    return;
                }

                // UserId und UserName vom authentifizierten User setzen
                e.From_UserId = (int)senderUser.UserID!;
                e.From_UserName = senderUser.Name;
                e.date = DateTime.Now;

                // Nachricht in der DB speichern
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db\\chat.db");
                var options = new DataOptions()
                    .UseSQLite($"Data Source={path}");
                using (var db = new ChatDB(options))
                {
                    db.Messages.Insert(() => new DataModels.Message
                    {
                        Text = e.Message_Text,
                        FromID = e.From_UserId,
                        ToID = e.To_RommId,
                        Date = e.date
                    });
                }

                // In-Memory-Liste aktualisieren
                messages.Add(new DataModels.Message
                {
                    Text = e.Message_Text,
                    FromID = e.From_UserId,
                    ToID = e.To_RommId,
                    Date = e.date
                });

                // Nachricht an alle authentifizierten Clients senden, die im Room sind
                foreach (var kvp in clientUserMap.ToList())
                {
                    var clientTransfer = kvp.Key;
                    var clientUser = kvp.Value;

                    // Prüfen ob dieser User Mitglied im Ziel-Room ist
                    bool isInRoom = userRooms.Any(ur =>
                        ur.UserID == clientUser.UserID &&
                        ur.RoomID == e.To_RommId);

                    if (isInRoom)
                    {
                        try
                        {
                            clientTransfer.SendMessage(new Message
                            {
                                Type = 1,
                                Message_Text = e.Message_Text,
                                From_UserId = e.From_UserId,
                                From_UserName = e.From_UserName,
                                To_RommId = e.To_RommId,
                                date = e.date
                            });
                        }
                        catch (Exception)
                        {
                            // Client disconnected, wird durch OnDisconnect aufgeräumt
                        }
                    }
                }

                Log($"Message from {e.From_UserName} to Room {e.To_RommId} broadcast");
                return;
            }
        }

        private void Log(string text)
        {
            Dispatcher.Invoke(() =>
            {
                LogBox.Items.Add(text);
            });
        }
    }
}