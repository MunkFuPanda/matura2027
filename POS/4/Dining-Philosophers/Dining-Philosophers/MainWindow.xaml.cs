using System.Collections.ObjectModel;
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

namespace Dining_Philosophers
{
    public partial class MainWindow : Window
    {
        bool started = false;
        public TextBox[] textboxes = new TextBox[5];
        public philosopher[] philosophers = new philosopher[5];
        public Thread[] threads = new Thread[5];
        public fork[] forks = new fork[5];
        public MainWindow()
        {
            InitializeComponent();

            textboxes = [status1, status2, status3, status4, status5];

            for (int i = 0; i < 5; i++)
            {
                forks[i] = new fork(i);
            }
        }

        private void buttonStart(Object sender, RoutedEventArgs e)
        {   
            philosophers[0] = new philosopher(0, status1, forks[0], forks[1], int.Parse(thinkingTime.Text), int.Parse(thinkingVariance.Text), int.Parse(eatingTime.Text), int.Parse(eatingVariance.Text), int.Parse(pickUpTime.Text));
            philosophers[1] = new philosopher(1, status2, forks[1], forks[2], int.Parse(thinkingTime.Text), int.Parse(thinkingVariance.Text), int.Parse(eatingTime.Text), int.Parse(eatingVariance.Text), int.Parse(pickUpTime.Text));
            philosophers[2] = new philosopher(2, status3, forks[2], forks[3], int.Parse(thinkingTime.Text), int.Parse(thinkingVariance.Text), int.Parse(eatingTime.Text), int.Parse(eatingVariance.Text), int.Parse(pickUpTime.Text));
            philosophers[3] = new philosopher(3, status4, forks[3], forks[4], int.Parse(thinkingTime.Text), int.Parse(thinkingVariance.Text), int.Parse(eatingTime.Text), int.Parse(eatingVariance.Text), int.Parse(pickUpTime.Text));
            philosophers[4] = new philosopher(4, status5, forks[4], forks[0], int.Parse(thinkingTime.Text), int.Parse(thinkingVariance.Text), int.Parse(eatingTime.Text), int.Parse(eatingVariance.Text), int.Parse(pickUpTime.Text));

            threads[0] = new Thread(new ThreadStart(philosophers[0].begin));
            threads[1] = new Thread(new ThreadStart(philosophers[1].begin));
            threads[2] = new Thread(new ThreadStart(philosophers[2].begin));
            threads[3] = new Thread(new ThreadStart(philosophers[3].begin));
            threads[4] = new Thread(new ThreadStart(philosophers[4].begin));

            for (int i = 0; i < 5; i++)
            {
                threads[i].Start();
            }
        }
        
        private void buttonEnd(Object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                threads[i].Interrupt();
            }
        }
     }
}