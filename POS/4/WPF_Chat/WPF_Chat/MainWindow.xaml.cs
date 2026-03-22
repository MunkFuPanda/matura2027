using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChatShared;
using Microsoft.Win32;

namespace WPF_Chat
{
    public partial class MainWindow : Window
    {
        private ChatClient _client = null!;
        private readonly Dictionary<string, ListBox> _roomListBoxes = new();
        private readonly Dictionary<string, BitmapImage> _profileImageCache = new();
        private readonly HashSet<string> _pendingImageRequests = [];
        private bool _colorPickerInitialized;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow { Owner = this };
            if (loginWindow.ShowDialog() == true && loginWindow.Client != null)
            {
                _client = loginWindow.Client;
                Title = $"WPF Chat - {_client.Username}";
                _client.OnMessageReceived += msg => Dispatcher.Invoke(() => HandleMessage(msg));
                _client.OnDisconnected += reason => Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Disconnected: {reason}", "Disconnected",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    Close();
                });

                if (_client.UserColor != null)
                {
                    try
                    {
                        ColorPicker.SelectedColor =
                            (Color)ColorConverter.ConvertFromString(_client.UserColor);
                    }
                    catch { }
                }
                _colorPickerInitialized = true;

                _ = _client.SendAsync(new ProtocolMessage { Type = MessageType.ListRoomsRequest });
            }
            else
            {
                Close();
            }
        }

        private void HandleMessage(ProtocolMessage msg)
        {
            switch (msg.Type)
            {
                case MessageType.ListRoomsResponse:
                    LstRooms.Items.Clear();
                    if (msg.Rooms != null)
                        foreach (var room in msg.Rooms)
                            LstRooms.Items.Add(room);
                    break;

                case MessageType.CreateRoomResponse:
                    if (!msg.Success)
                        MessageBox.Show(msg.ErrorMessage ?? "Failed to create room.");
                    break;

                case MessageType.JoinRoomResponse:
                    if (msg.Success && msg.RoomName != null)
                        AddChatTab(msg.RoomName, msg.Messages);
                    break;

                case MessageType.LeaveRoomResponse:
                    if (msg.Success && msg.RoomName != null)
                        RemoveChatTab(msg.RoomName);
                    break;

                case MessageType.ChatMessageBroadcast:
                    if (msg.RoomName != null && _roomListBoxes.TryGetValue(msg.RoomName, out var lb))
                        AddMessageToListBox(lb, msg.FromUsername!, msg.Content!, msg.Timestamp, msg.Color);
                    break;

                case MessageType.PrivateMessageReceived:
                    HandlePrivateMessageReceived(msg);
                    break;

                case MessageType.OnlineUsers:
                    LstOnlineUsers.Items.Clear();
                    if (msg.Users != null)
                        foreach (var user in msg.Users)
                            LstOnlineUsers.Items.Add(user);
                    break;

                case MessageType.ProfileImageData:
                    if (msg.Username != null && msg.ImageBase64 != null)
                    {
                        var img = Base64ToBitmapImage(msg.ImageBase64);
                        if (img != null)
                            _profileImageCache[msg.Username] = img;
                    }
                    break;

                case MessageType.UploadProfileImageResponse:
                    if (msg.Success)
                        MessageBox.Show("Profile image updated.", "Success");
                    break;
            }
        }

        private void AddChatTab(string roomName, List<ChatMessageData>? history)
        {
            if (_roomListBoxes.ContainsKey(roomName)) return;

            var listBox = new ListBox
            {
                ItemTemplate = CreateMessageTemplate(),
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };

            if (history != null)
            {
                foreach (var m in history)
                    AddMessageToListBox(listBox, m.FromUsername!, m.Content!, m.Timestamp, m.Color);
            }

            var tabItem = new TabItem
            {
                Header = CreateTabHeader(roomName),
                Content = listBox,
                Tag = roomName
            };

            _roomListBoxes[roomName] = listBox;
            TabChat.Items.Add(tabItem);
            TabChat.SelectedItem = tabItem;
        }

        private object CreateTabHeader(string roomName)
        {
            var sp = new StackPanel { Orientation = Orientation.Horizontal };
            sp.Children.Add(new TextBlock { Text = roomName, Margin = new Thickness(0, 0, 5, 0) });
            var closeBtn = new Button
            {
                Content = "×",
                FontSize = 10,
                Padding = new Thickness(2, 0, 2, 0),
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Cursor = Cursors.Hand,
                Tag = roomName
            };
            closeBtn.Click += (s, e) =>
            {
                var room = (string)((Button)s!).Tag;
                _ = _client.SendAsync(new ProtocolMessage
                {
                    Type = MessageType.LeaveRoomRequest,
                    RoomName = room
                });
            };
            sp.Children.Add(closeBtn);
            return sp;
        }

        private void RemoveChatTab(string roomName)
        {
            _roomListBoxes.Remove(roomName);
            var tab = TabChat.Items.Cast<TabItem>().FirstOrDefault(t => (string)t.Tag == roomName);
            if (tab != null) TabChat.Items.Remove(tab);
        }

        private void AddMessageToListBox(ListBox listBox, string from, string content,
            DateTime timestamp, string? color)
        {
            var item = new MessageItem
            {
                Username = from,
                Content = content,
                Timestamp = timestamp.ToString("HH:mm:ss"),
                Color = color ?? "#FF000000"
            };

            if (_profileImageCache.TryGetValue(from, out var img))
            {
                item.ProfileImage = img;
            }
            else if (_pendingImageRequests.Add(from))
            {
                _ = _client.SendAsync(new ProtocolMessage
                {
                    Type = MessageType.RequestProfileImage,
                    Username = from
                });
            }

            listBox.Items.Add(item);
            listBox.ScrollIntoView(item);
        }

        private DataTemplate CreateMessageTemplate()
        {
            var template = new DataTemplate(typeof(MessageItem));

            var factory = new FrameworkElementFactory(typeof(StackPanel));
            factory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            factory.SetValue(StackPanel.MarginProperty, new Thickness(2));

            // Profile image
            var imgFactory = new FrameworkElementFactory(typeof(Image));
            imgFactory.SetBinding(Image.SourceProperty, new Binding("ProfileImage"));
            imgFactory.SetValue(Image.WidthProperty, 32.0);
            imgFactory.SetValue(Image.HeightProperty, 32.0);
            imgFactory.SetValue(Image.MarginProperty, new Thickness(0, 0, 5, 0));
            factory.AppendChild(imgFactory);

            var textPanel = new FrameworkElementFactory(typeof(StackPanel));

            // Username + timestamp header
            var headerPanel = new FrameworkElementFactory(typeof(StackPanel));
            headerPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            var usernameFactory = new FrameworkElementFactory(typeof(TextBlock));
            usernameFactory.SetBinding(TextBlock.TextProperty, new Binding("Username"));
            usernameFactory.SetBinding(TextBlock.ForegroundProperty, new Binding("BrushColor"));
            usernameFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            usernameFactory.SetValue(TextBlock.MarginProperty, new Thickness(0, 0, 5, 0));
            headerPanel.AppendChild(usernameFactory);

            var timeFactory = new FrameworkElementFactory(typeof(TextBlock));
            timeFactory.SetBinding(TextBlock.TextProperty, new Binding("Timestamp"));
            timeFactory.SetValue(TextBlock.ForegroundProperty, Brushes.Gray);
            timeFactory.SetValue(TextBlock.FontSizeProperty, 10.0);
            headerPanel.AppendChild(timeFactory);

            textPanel.AppendChild(headerPanel);

            // Message content
            var contentFactory = new FrameworkElementFactory(typeof(TextBlock));
            contentFactory.SetBinding(TextBlock.TextProperty, new Binding("Content"));
            contentFactory.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            textPanel.AppendChild(contentFactory);

            factory.AppendChild(textPanel);
            template.VisualTree = factory;
            return template;
        }

        private void HandlePrivateMessageReceived(ProtocolMessage msg)
        {
            string pmTabName = msg.FromUsername == _client.Username
                ? $"PM: {msg.ToUsername}"
                : $"PM: {msg.FromUsername}";

            if (!_roomListBoxes.ContainsKey(pmTabName))
                AddPmTab(pmTabName);

            if (_roomListBoxes.TryGetValue(pmTabName, out var lb))
                AddMessageToListBox(lb, msg.FromUsername!, msg.Content!, msg.Timestamp, msg.Color);
        }

        private void AddPmTab(string pmTabName)
        {
            var listBox = new ListBox
            {
                ItemTemplate = CreateMessageTemplate(),
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };

            var tabItem = new TabItem
            {
                Header = CreateTabHeader(pmTabName),
                Content = listBox,
                Tag = pmTabName
            };

            _roomListBoxes[pmTabName] = listBox;
            TabChat.Items.Add(tabItem);
        }

        private async void LstRooms_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LstRooms.SelectedItem is string roomName)
            {
                await _client.SendAsync(new ProtocolMessage
                {
                    Type = MessageType.JoinRoomRequest,
                    RoomName = roomName
                });
            }
        }

        private async void BtnCreateRoom_Click(object sender, RoutedEventArgs e)
        {
            var roomName = TxtNewRoom.Text.Trim();
            if (string.IsNullOrEmpty(roomName)) return;

            await _client.SendAsync(new ProtocolMessage
            {
                Type = MessageType.CreateRoomRequest,
                RoomName = roomName
            });
            TxtNewRoom.Clear();
        }

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            await SendCurrentMessage();
        }

        private async void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) await SendCurrentMessage();
        }

        private async Task SendCurrentMessage()
        {
            var content = TxtMessage.Text.Trim();
            if (string.IsNullOrEmpty(content)) return;
            if (TabChat.SelectedItem is not TabItem tab) return;

            var roomName = (string)tab.Tag;

            if (roomName.StartsWith("PM: "))
            {
                var toUser = roomName[4..];
                await _client.SendAsync(new ProtocolMessage
                {
                    Type = MessageType.PrivateMessage,
                    ToUsername = toUser,
                    Content = content
                });
            }
            else
            {
                await _client.SendAsync(new ProtocolMessage
                {
                    Type = MessageType.ChatMessage,
                    RoomName = roomName,
                    Content = content
                });
            }
            TxtMessage.Clear();
        }

        private async void BtnSendPM_Click(object sender, RoutedEventArgs e)
        {
            if (LstOnlineUsers.SelectedItem is not string toUser) return;
            var content = TxtPrivateMessage.Text.Trim();
            if (string.IsNullOrEmpty(content)) return;

            string pmTabName = $"PM: {toUser}";
            if (!_roomListBoxes.ContainsKey(pmTabName))
                AddPmTab(pmTabName);

            await _client.SendAsync(new ProtocolMessage
            {
                Type = MessageType.PrivateMessage,
                ToUsername = toUser,
                Content = content
            });
            TxtPrivateMessage.Clear();

            var tab = TabChat.Items.Cast<TabItem>().FirstOrDefault(t => (string)t.Tag == pmTabName);
            if (tab != null) TabChat.SelectedItem = tab;
        }

        private async void MenuProfileImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.gif"
            };
            if (dialog.ShowDialog() == true)
            {
                var base64 = ImageFileToBase64(dialog.FileName);
                if (base64 != null)
                {
                    var profileImage = Base64ToBitmapImage(base64);
                    if (_client.Username != null && profileImage != null)
                        _profileImageCache[_client.Username] = profileImage;

                    await _client.SendAsync(new ProtocolMessage
                    {
                        Type = MessageType.UploadProfileImage,
                        ImageBase64 = base64
                    });
                }
            }
        }

        private async void ColorPicker_SelectedColorChanged(object sender,
            RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!_colorPickerInitialized) return;
            if (_client == null || !_client.IsConnected) return;
            if (e.NewValue.HasValue)
            {
                string colorStr = e.NewValue.Value.ToString();
                _client.UserColor = colorStr;
                await _client.SendAsync(new ProtocolMessage
                {
                    Type = MessageType.UpdateColor,
                    Color = colorStr
                });
            }
        }

        private static string? ImageFileToBase64(string filePath)
        {
            try
            {
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.DecodePixelWidth = 50;
                bi.UriSource = new Uri(filePath);
                bi.EndInit();

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bi));
                using var ms = new MemoryStream();
                encoder.Save(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return null;
            }
        }

        private static BitmapImage? Base64ToBitmapImage(string base64)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = new MemoryStream(bytes);
                bi.EndInit();
                bi.Freeze();
                return bi;
            }
            catch
            {
                return null;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _client?.Dispose();
        }
    }

    public class MessageItem
    {
        public string Username { get; set; } = "";
        public string Content { get; set; } = "";
        public string Timestamp { get; set; } = "";
        public string Color { get; set; } = "#FF000000";
        public BitmapImage? ProfileImage { get; set; }

        public SolidColorBrush BrushColor
        {
            get
            {
                try { return new SolidColorBrush((Color)ColorConverter.ConvertFromString(Color)); }
                catch { return Brushes.Black; }
            }
        }
    }
}