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
using Hardcodet.Wpf.TaskbarNotification;

namespace SystemMonitor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window {
        private Thread _worker;
        private bool _isRunning = false;
        private SystemData _systemData;
        private bool isVisible = true;
        private const int historyLength = 50;
        private readonly List<double> _history = new();
        public MainWindow() {
            InitializeComponent();
            _systemData = new SystemData();
            _isRunning = true;
            _worker = new Thread(UpdateLoop);
            _worker.IsBackground = true;
            _worker.Start();

            var tray = (TaskbarIcon)Resources["TrayIcon"];
        }

        private void UpdateLoop() {
            while (_isRunning) {
                double cpuPercent = _systemData.GetProcessorPercent();
                this.Dispatcher.Invoke(() => {
                    indicator.CurrentValue = cpuPercent;

                    _history.Add(cpuPercent);
                    if (_history.Count > historyLength) {
                        _history.RemoveAt(0);
                    }
                });
                Thread.Sleep(1000);
            }
        }

        private void ToggleVisibility_Click(object sender, RoutedEventArgs e) {
            if (isVisible) {
                this.Hide();
                isVisible = false;
            } else {
                this.Show();
                isVisible = true;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            _isRunning = false;
        }
    }
}