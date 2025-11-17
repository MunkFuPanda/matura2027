using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Primzahlgenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            image1.Visibility = Visibility.Hidden;
            this.WindowState = WindowState.Maximized;
            result.Height = this.Height;
        }

        private void start_button(object sender, RoutedEventArgs e)
        {
            result.Items.Clear();
            ThreadPool.QueueUserWorkItem(new WaitCallback(PrimWorker), count.Text);
        }

        private void reset_view(object sender, RoutedEventArgs e)
        {
            result.Items.Clear();
        }

        private void PrimWorker(object data)
        {
            int count = int.Parse(data.ToString());
            Storyboard r = new Storyboard();
            
            image1.Dispatcher.Invoke(() =>
            {
                image1.Visibility = Visibility.Visible;
                r = (Storyboard)FindResource("loadingRotation");
                r.Begin(this, true);
            });

            List<int> primes = new List<int>();
            int number = 2;

            while (primes.Count < count)
            {
                Thread.Sleep(1);
                bool isPrime = true;

                for (int i = 2; i * i <= number; i++)
                {
                    if (number % i == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime) { 
                    primes.Add(number);
                    result.Dispatcher.Invoke(() =>
                    {
                        result.Items.Add(number);
                    });
                }
                number++;
            }
            image1.Dispatcher.Invoke(() =>
            {
                r.Stop(this);
                image1.Visibility = Visibility.Hidden;
                result_label.Content = primes.Last();
            });
        }
    }
}