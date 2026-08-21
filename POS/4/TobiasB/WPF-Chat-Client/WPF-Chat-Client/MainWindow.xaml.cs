using NetworkLibrary;
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

namespace WPF_Chat_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient client;

        private Transfer<Paket> transfer;

        private UserDto current_user;

        public MainWindow()
        {
            InitializeComponent();

            Thread thread = new Thread(delegate ()
            {
                client = new TcpClient("localhost", 12345);

                this.Dispatcher.Invoke(new Action(() =>
                {
                    debuglabel.Content = "Server connected!: " + client.Client.RemoteEndPoint.ToString();
                }));

                StartTransfer();

            });
            thread.Start();

        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (client == null)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                input.Text = "";
                ChatUser.Items.Clear();
                lb_messages.Items.Clear();
            });

            LoginDialog loginDialog = new LoginDialog();
            loginDialog.ShowDialog();
            if (loginDialog.Ok == true)
            {
                Paket userlogin = new Paket(MessageType.Login, loginDialog.UserName, loginDialog.Password);
                
                transfer.SendMessage(userlogin);

                transfer.SendMessage(new Paket(MessageType.GetUsers));

            }
        }

        private void StartTransfer()
        {
            transfer = new Transfer<Paket>(client);

            transfer.OnMessageReceived += (sender, e) =>
            {
                if (e.type == MessageType.FailedLogin)
                {
                    MessageBox.Show("Falsche Logindaten", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (e.type == MessageType.FailedRegistration)
                {
                    MessageBox.Show("Fehler beim Registrieren", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (e.type == MessageType.MessageReceive)
                {
                    foreach (MessageDto m in e.messages)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            lb_messages.Items.Add(m);
                        });
                    }
                }

                if (e.type == MessageType.Login)
                {
                    current_user = e.users.First();
                }

                if (e.type == MessageType.GetUsers)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        ChatUser.Items.Clear();
                    }));

                    foreach (UserDto u in e.users)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            ChatUser.Items.Add(u);
                        }));

                    }

                    
                }
            };
        }

        private void registrationButton_Click(object sender, RoutedEventArgs e)
        {
            if (client == null)
            {
                return;
            }

            RegistrationDialog regDialog = new RegistrationDialog();
            regDialog.ShowDialog();
            if (regDialog.Ok == true)
            {
                Paket userreg = new Paket(MessageType.Registration, regDialog.UserName, regDialog.Password);

                transfer.SendMessage(userreg);

            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(input.Text))
            {
                return;
            }

            

            MessageDto message = new MessageDto();
            message.Titel = input.Text;
            message.Content = input.Text;
            message.Sender = (long)current_user.ID;

            this.Dispatcher.Invoke(new Action(() =>
            {
                lb_messages.Items.Add(message);
            }));

            this.Dispatcher.Invoke(new Action(() =>
            {
                input.Text = "";
            }));

            UserDto receiv = (UserDto)ChatUser.SelectedItem;

            message.Receiver = receiv.ID;
            message.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Paket msg = new Paket(MessageType.MessageSend, new List<MessageDto>() { message });
            transfer.SendMessage(msg);
        }

        private void ChatUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (transfer == null)
                return;

            if (current_user == null)
                return;

            if (ChatUser.SelectedItem == null)
                return;

            try
            {
                UserDto userDto = (UserDto)ChatUser.SelectedItem;
                Paket paket = new Paket(MessageType.MessageReceive, userDto.ID, current_user.ID);

                this.Dispatcher.Invoke(new Action(() =>
                {
                    lb_messages.Items.Clear();
                }));

                transfer.SendMessage(paket);
            }
            catch (Exception ex)
            {
                return;
            }

            
        }
    }
}