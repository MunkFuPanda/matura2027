using LinqToDB;
using NetworkLibrary;
using DataModels;
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

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TcpClient client;
        public Transfer<Msg> transfer;

        DbDB db;
        
        public MainWindow()
        {
            InitializeComponent();

            ThreadPool.QueueUserWorkItem(Connect);

            // DB KOPIEREN WENN NEUER WICHTIG

            // Namespace aufpassen!!!

            db = new DbDB(new DataOptions().UseSQLite(@"Data Source=db.db"));

            User u1 = new User();
            u1.Name = "hto";

            db.Insert(u1);

            DataModels.User u2 = db.Users.Where(x => x.Name == "hto").FirstOrDefault();

            MessageBox.Show(u2.Name);


        }

        private void Connect(object o)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();
            client = listener.AcceptTcpClient();

            MessageBox.Show(client.Client.RemoteEndPoint.ToString());

            transfer = new Transfer<Msg>(client);

        }
    }
}

