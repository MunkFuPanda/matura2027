using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetworkLibrary;

namespace _3tePAUebung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        TcpClient client;
        Transfer<Msg> transfer;

        public MainWindow()
        {
            InitializeComponent();

            ThreadPool.QueueUserWorkItem(Connect);
        }

        private void Connect(object o)
        {
            client = new TcpClient("localhost", 12345);
            MessageBox.Show(client.Client.RemoteEndPoint.ToString());
            transfer = new Transfer<Msg>(client);

            transfer.OnMessageReceived += Transfer_OnMessageReceived;

        }

        private void Transfer_OnMessageReceived(object? sender, Msg e)
        {
           if (e.msgenum == Msgenum.Login)
            {
                transfer.SendMessage(new Msg(Msgenum.GetUserList, null, null, null, null));
            }
           if (e.msgenum)
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loadMessageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}