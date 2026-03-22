using System.Windows;

namespace WPF_Chat_Server;

public partial class MainWindow : Window
{
    private Database? _db;
    private ChatServer? _server;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void BtnStart_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(TxtPort.Text, out int port))
        {
            MessageBox.Show("Invalid port number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _db = new Database();
        _server = new ChatServer(_db);
        _server.OnLog += msg => Dispatcher.Invoke(() =>
        {
            LstLog.Items.Add(msg);
            LstLog.ScrollIntoView(LstLog.Items[^1]);
        });

        BtnStart.IsEnabled = false;
        BtnStop.IsEnabled = true;
        TxtPort.IsEnabled = false;

        await _server.StartAsync(port);
    }

    private void BtnStop_Click(object sender, RoutedEventArgs e)
    {
        _server?.Stop();
        _db?.Dispose();
        _db = null;
        _server = null;
        BtnStart.IsEnabled = true;
        BtnStop.IsEnabled = false;
        TxtPort.IsEnabled = true;
    }

    protected override void OnClosed(EventArgs e)
    {
        _server?.Stop();
        _db?.Dispose();
        base.OnClosed(e);
    }
}
