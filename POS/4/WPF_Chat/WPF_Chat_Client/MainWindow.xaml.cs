using NetworkLibrary;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace WPF_Chat_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class RoomViewModel : INotifyPropertyChanged
    {
        public int RoomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ObservableCollection<MessageViewModel> Messages { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class MessageViewModel
    {
        public string SenderName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
    }

    public class ClientViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<RoomViewModel> Rooms { get; set; } = new();

        private RoomViewModel? _selectedRoom;
        public RoomViewModel? SelectedRoom
        {
            get => _selectedRoom;
            set { _selectedRoom = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public RoomViewModel GetOrCreateRoom(int roomId, string roomName)
        {
            var existing = Rooms.FirstOrDefault(r => r.RoomId == roomId);
            if (existing != null)
                return existing;

            var room = new RoomViewModel { RoomId = roomId, Name = roomName };
            Rooms.Add(room);
            return room;
        }

        public void AddMessageToRoom(int roomId, MessageViewModel message)
        {
            var room = Rooms.FirstOrDefault(r => r.RoomId == roomId);
            room?.Messages.Add(message);
        }
    }

    public partial class MainWindow : Window
    {
        TcpClient client;
        Transfer<Message> transfer;
        List<Message> messages = new List<Message>();
        Dictionary<int, string> rooms = new Dictionary<int, string>();
        ClientViewModel viewModel = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;

            try
            {
                client = new TcpClient("localhost", 12345);
                transfer = new Transfer<Message>(client);
                transfer.OnMessageReceived += Transfer_OnMessageReceived;
                transfer.OnDisconnect += Transfer_OnDisconnect;

                var dialog = new Login();
                if (dialog.ShowDialog() == true)
                {
                    string username = dialog.Username;
                    string password = dialog.Password;
                    transfer.SendMessage(new Message((int)MessageType.Authenticate, $"{username}:{password}"));
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Could not connect to server. Make sure the server is running.");
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void Transfer_OnMessageReceived(object? sender, Message e)
        {
            if (e.Type == (int)MessageType.Rooms)
            {
                // Rooms kommen als "id1;name1:id2;name2:..."
                string[] room_string = e.Message_Text.Split(':');
                Dispatcher.Invoke(() =>
                {
                    foreach (var r in room_string)
                    {
                        string[] room_info = r.Split(';');
                        if (room_info.Length == 2 && int.TryParse(room_info[0], out int roomId))
                        {
                            rooms[roomId] = room_info[1];
                            viewModel.GetOrCreateRoom(roomId, room_info[1]);
                        }
                    }

                    // Ersten Room auswählen falls noch keiner selektiert
                    if (viewModel.SelectedRoom == null && viewModel.Rooms.Count > 0)
                        viewModel.SelectedRoom = viewModel.Rooms[0];
                });
            }
            else if (e.Type == (int)MessageType.ChatMessage)
            {
                messages.Add(e);

                Dispatcher.Invoke(() =>
                {
                    // Room ggf. anlegen falls noch nicht vorhanden
                    string roomName = rooms.ContainsKey(e.To_RommId)
                        ? rooms[e.To_RommId]
                        : $"Room {e.To_RommId}";
                    viewModel.GetOrCreateRoom(e.To_RommId, roomName);

                    viewModel.AddMessageToRoom(e.To_RommId, new MessageViewModel
                    {
                        SenderName = e.From_UserName,
                        Text = e.Message_Text,
                        Date = e.date
                    });
                });
            }
        }

        private void Transfer_OnDisconnect(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Disconnected from server.");
            });
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(InputBox.Text) || viewModel.SelectedRoom == null)
                return;

            transfer.SendMessage(new Message(
                (int)MessageType.ChatMessage,
                InputBox.Text,
                0, // From_UserId – wird vom Server gesetzt
                viewModel.SelectedRoom.RoomId,
                DateTime.Now
            ));

            InputBox.Clear();
        }
    }
}