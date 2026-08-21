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

namespace Die_speisenden_Philosophen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int button_pressed = 0;

        private List<Philosopher> philosophers = new List<Philosopher>();
        private List<Thread> philosophers_thread = new List<Thread>();

        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (button_pressed == 1)
            {
                return;
            }

            button_pressed = 1;

            // check for null values

            Fork f0 = new Fork(0);
            Fork f1 = new Fork(1);
            Fork f2 = new Fork(2);
            Fork f3 = new Fork(3);
            Fork f4 = new Fork(4);

            Philosopher p0 = new Philosopher(0, status_1, f0, f1);
            p0.AVG_THINK = int.Parse(tb_avg_think.Text);
            p0.VAR_THINK = int.Parse(tb_var_think.Text);
            p0.AVG_EAT = int.Parse(tb_avg_eat.Text);
            p0.VAR_EAT = int.Parse(tb_var_eat.Text);
            p0.TAKE_FORK = int.Parse(tb_take_fork.Text);

            Philosopher p1 = new Philosopher(1, status_2, f1, f2);
            Philosopher p2 = new Philosopher(2, status_3, f2, f3);
            Philosopher p3 = new Philosopher(3, status_4, f3, f4);
            Philosopher p4 = new Philosopher(4, status_5, f4, f0);

            Thread thp0 = new Thread(p0.ThinkingEatingPhilosopher);
            Thread thp1 = new Thread(p1.ThinkingEatingPhilosopher);
            Thread thp2 = new Thread(p2.ThinkingEatingPhilosopher);
            Thread thp3 = new Thread(p3.ThinkingEatingPhilosopher);
            Thread thp4 = new Thread(p4.ThinkingEatingPhilosopher);

            philosophers_thread.Add(thp0);
            philosophers_thread.Add(thp1);
            philosophers_thread.Add(thp2);
            philosophers_thread.Add(thp3);
            philosophers_thread.Add(thp4);

            philosophers.Add(p0);
            philosophers.Add(p1);
            philosophers.Add(p2);
            philosophers.Add(p3);
            philosophers.Add(p4);


            thp0.Start();
            thp1.Start();
            thp2.Start();
            thp3.Start();
            thp4.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            button_pressed = 0;

            foreach (var p in philosophers)
            {
                p.I_END = 1;
            }

            foreach (var p in philosophers_thread)
            {
                p.Interrupt();
                p.Join();
            }

            status_1.Content = "Status";
            status_2.Content = "Status";
            status_3.Content = "Status";
            status_4.Content = "Status";
            status_5.Content = "Status";

            status_1.Background = Brushes.White;
            status_2.Background = Brushes.White;
            status_3.Background = Brushes.White;
            status_4.Background = Brushes.White;
            status_5.Background = Brushes.White;
        }
    }
}