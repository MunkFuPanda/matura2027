using Hardcodet.Wpf.TaskbarNotification;
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

namespace SystemMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread _worker;
        private bool _running;
        private SystemData _systemData;
        private bool shown = true;
        private const int MaxPoints = 50;
        private readonly List<double> _history = new();


        public MainWindow()
        {
            InitializeComponent();
            _systemData = new SystemData();
            _running = true;

            _worker = new Thread(UpdateLoop);
            _worker.IsBackground = true;
            _worker.Start();
            var tray = (TaskbarIcon)Resources["TrayIcon"];

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void UpdateLoop()
        {
            while (_running)
            {
                double value = _systemData.GetProcessorPercent();

                Dispatcher.Invoke(() =>
                {
                    indicator.CurrentValue = (int)value;

                    _history.Add(value);
                    if (_history.Count > MaxPoints)
                        _history.RemoveAt(0);

                    //UpdateGraph();
                });

                Thread.Sleep(1000);
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            _running = false;
            base.OnClosed(e);
        }

        private void ToggleVisibility(object sender, RoutedEventArgs e)
        {
            if (shown)
            {
                this.Hide();
                shown = !shown;
            }
            else
            {
                this.Show();
                shown = !shown;
            }
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /*
        private void UpdateGraph()
        {
            GraphLine.Points.Clear();

            double width = GraphView.Width;
            double height = GraphView.Height;

            for (int i = 0; i < _history.Count; i++)
            {
                double x = i * (width / MaxPoints);
                double y = height - (_history[i] / 100.0 * height);

                GraphLine.Points.Add(new Point(x, y));
            }
        }
        */

        private void ShowIndicator(object sender, RoutedEventArgs e)
        {
            indicator.Visibility = Visibility.Visible;
            //GraphView.Visibility = Visibility.Collapsed;
        }

        private void ShowGraph(object sender, RoutedEventArgs e)
        {
            indicator.Visibility = Visibility.Collapsed;
            //GraphView.Visibility = Visibility.Visible;
        }
    }
}