using System.IO;
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
using SystemMonitor;

namespace WPF_System_Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            ThreadPool.QueueUserWorkItem(new WaitCallback(Worker), null);
        }

        private void Worker(object obj)
        {
            SystemData data = new SystemData();
            while (true)
            {
                
                this.Dispatcher.Invoke(new Action(() =>
                {
                    Indicator.Value = (int)data.GetProcessorPercent();
                }));
                Thread.Sleep(350);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MenuItem_Click_Hidden_Visible(object sender, RoutedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Visibility = Visibility.Visible;
            }
                
        }

        private void MenuItem_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}