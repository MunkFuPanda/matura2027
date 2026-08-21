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

namespace Achterbahn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            int person_count = int.Parse(tb_count_person.Text);
            int places_count = int.Parse(tb_count_places.Text);


            cart c = new cart(places_count, 5, lb_status_achterbahn);

            for (int i = 0; i < person_count;  i++)
            {
                rollercoaster p = new rollercoaster(i, c, waiting_queue, roller_coaster_queue, walking_queue);
                Thread t = new Thread(new ThreadStart(p.Person));
                t.Start();
            }

            Thread tc = new Thread(new ThreadStart(c.CartThread));
            tc.Start();
        }
    }
}