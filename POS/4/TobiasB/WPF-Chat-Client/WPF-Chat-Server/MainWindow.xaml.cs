using DataModels; // vom visual studio tool
// using DataModel vom .net tool
using LinqToDB;
using LinqToDB.Data;
using NetworkLibrary;
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
using System.Windows.Shapes;

namespace WPF_Chat_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TcpClient client;
        private ChatDB db;

        public MainWindow()
        {
            InitializeComponent();

            // db kopieren wenn neuer
            // Namespace beachten falls man 2 User hat wegen dto Dto.User, DataModels.user

            // Wenn DB nicht geht
            // Visual Studio neu starten
            // dann geht relativer Pfad
            // notfalls kompletten pfad angeben
            // notfalls version downgraden 6.1.0


            // try catch auch bei netzwerk bei client und server allgemein und abfangen, damit nichts crasht
            // keine verbindung konnte hergestellt werden

            // VS Model verwenden ist besser, tt, copyme, und dann dateipfad und dann speichern, dann aufklappen, dann hat
            // man die Datei

            // chat.db bei eigenschaftsfenster auf kopieren wenn neuer damit man die datenbank findet
            // wenn etwas nicht funktioniert mit der db, kaputt, falsche Daten, einfach immer kopieren
            // einmal starten und dann wieder zurück auf kopieren wenn neuer
            // alter stand wurde überschrieben

            db = new ChatDB(new DataOptions().UseSQLite(@"Data Source=chat.db"));

            //User donald = db.Users.Where(x => x.Name == "Donald").LoadWith(x => x.Fk_ReceivedMessages).FirstOrDefault();

            //DataModels.Message message = db.Messages.Where(x => x.Receiver == donald.ID).LoadWith(x => x.FK_Sender).LoadWith(x => x.FK_Sender.RoomUsers).FirstOrDefault();

            //foreach (User user in users.Where(x => x.name == "Tobias"))
            //{
            //    MessageBox.Show(user.name);
            //}

            //User u = new User { name = "Sebastian", password = "Munkhbat" };

            //db.Insert(u);

            // db.Close();


            TcpListener server = new TcpListener(IPAddress.Any, 12345);

            Thread thread = new Thread(delegate ()
            {
                server.Start();
                client = server.AcceptTcpClient();

                this.Dispatcher.Invoke(new Action(() => {
                    debugListBox.Items.Add("Client connected!: " + client.Client.RemoteEndPoint.ToString());
                }));

                StartLogin();
            });
            thread.Start();

        }

        private void StartLogin()
        {
            Transfer<Paket> transfer = new Transfer<Paket>(client);

            bool trylogin = false;

            transfer.OnMessageReceived += (sender, e) =>
            {
                if (e.type == MessageType.Login)
                {

                    User loggin = db.Users.Where(x => x.Name == e.username && x.Password == e.password).FirstOrDefault();

                    if (loggin == null)
                    {
                        transfer.SendMessage(new Paket(MessageType.FailedLogin));
                        return;
                    }

                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        debugListBox.Items.Add("User " + loggin.Name + " hat sich gerade eingeloggt");
                    }));



                    transfer.SendMessage(new Paket(MessageType.Login, new List<UserDto>() { UserToDto(loggin) }));

                    

                }

                else if (e.type == MessageType.Registration)
                {
                    if (String.IsNullOrEmpty(e.username) || String.IsNullOrEmpty(e.password))
                    {
                        transfer.SendMessage(new Paket(MessageType.FailedRegistration));
                        return;
                    }

                    User ifexists = db.Users.Where(x => x.Name == e.username && x.Password == e.password).FirstOrDefault();

                    if (ifexists == null)
                    {
                        User registuser = new User();
                        registuser.Name = e.username;
                        registuser.Password = e.password;
                        db.Insert(registuser);
                    }



                    

                    //if (users2.Contains(new User(e.username, e.password)))
                    //{
                    //    transfer.SendMessage(new Message(MessageType.FailedRegistration));
                    //    return;
                    //}

                    //users2.Add(new User(e.username, e.password));

                    this.Dispatcher.Invoke(new Action(() => {
                        debugListBox.Items.Add("User " + e.username + " hat sich gerade registriert");
                    }));

                }

                else if (e.type == MessageType.GetUsers)
                {
                    List<User> users = db.Users.ToList();
                    List<UserDto> userDtos = new List<UserDto>();

                    foreach (User u in users)
                    {
                        userDtos.Add(UserToDto(u));
                    }

                    transfer.SendMessage(new Paket(MessageType.GetUsers, userDtos));
                }

                else if (e.type == MessageType.MessageSend)
                {
                    Message m = new Message();
                    m.Titel = e.messages.First().Titel;
                    m.Content = e.messages.First().Content;
                    m.Sender = e.messages.First().Sender;
                    m.Receiver = e.messages.First().Receiver;
                    m.Room = e.messages.First().Room;
                    m.Timestamp = e.messages.First().Timestamp;
                    db.Insert<Message>(m);
                }

                else if (e.type == MessageType.MessageReceive)
                {
                    List<Message> messages = db.Messages.Where(x => (x.Sender == e.sender && x.Receiver == e.receiver) || (x.Sender == e.receiver && x.Receiver == e.sender)).ToList();

                    List<MessageDto> messageDtos = new List<MessageDto>();

                    foreach (Message m in messages)
                    {
                        messageDtos.Add(MessageToDto(m));
                    }

                    transfer.SendMessage(new Paket(MessageType.MessageReceive, messageDtos));
                }

                else
                {
                    return;
                }
                
            };

            transfer.OnDisconnect += (sender, e) =>
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    debugListBox.Items.Add("Client disconnected");
                }));

            };
        }

        private MessageDto MessageToDto(Message m)
        {
            MessageDto mDto = new MessageDto();
            mDto.ID = m.ID;
            mDto.Titel = m.Titel;
            mDto.Content = m.Content;
            mDto.Sender = m.Sender;
            mDto.Receiver = m.Receiver;
            mDto.Room = m.Room;
            mDto.Timestamp = m.Timestamp;
            return mDto;
        }

        private UserDto UserToDto(User u)
        {
            UserDto uDto = new UserDto();
            uDto.ID = u.ID;
            uDto.Name = u.Name;
            uDto.Password = u.Password;
            uDto.Timestamp = u.Timestamp;
            return uDto;
        }

        private RoomDto RoomToDto(Room r)
        {
            RoomDto rDto = new RoomDto();
            rDto.ID = r.ID;
            rDto.Name = r.Name;
            return rDto;
        }

        private RoomUserDto RoomUserToDto(RoomUser ru)
        {
            RoomUserDto ruDto = new RoomUserDto();
            ruDto.Room = ru.Room;
            ruDto.User = ru.User;
            return ruDto;
        }
    }
}