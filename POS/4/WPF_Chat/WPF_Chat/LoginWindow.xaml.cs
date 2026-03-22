using System.Windows;

namespace WPF_Chat;

public partial class LoginWindow : Window
{
    public ChatClient? Client { get; private set; }
    public ClientSettings Settings { get; }

    public LoginWindow()
    {
        InitializeComponent();
        Settings = ClientSettings.Load();
        TxtServer.Text = Settings.ServerHost;
        TxtPort.Text = Settings.ServerPort.ToString();
        if (Settings.LastUsername != null)
            TxtUsername.Text = Settings.LastUsername;
    }

    private async void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        await ConnectAndAuthenticate(isRegister: false);
    }

    private async void BtnRegister_Click(object sender, RoutedEventArgs e)
    {
        await ConnectAndAuthenticate(isRegister: true);
    }

    private async Task ConnectAndAuthenticate(bool isRegister)
    {
        if (string.IsNullOrWhiteSpace(TxtUsername.Text) || string.IsNullOrWhiteSpace(TxtPassword.Password))
        {
            TxtStatus.Text = "Please enter username and password.";
            return;
        }

        if (!int.TryParse(TxtPort.Text, out int port))
        {
            TxtStatus.Text = "Invalid port number.";
            return;
        }

        BtnLogin.IsEnabled = false;
        BtnRegister.IsEnabled = false;
        TxtStatus.Text = "Connecting...";

        try
        {
            Client = new ChatClient();
            await Client.ConnectAsync(TxtServer.Text, port);

            if (isRegister)
            {
                var regResponse = await Client.RegisterAsync(TxtUsername.Text, TxtPassword.Password);
                if (!regResponse.Success)
                {
                    TxtStatus.Text = regResponse.ErrorMessage ?? "Registration failed.";
                    Client.Dispose();
                    Client = null;
                    return;
                }
            }

            var loginResponse = await Client.LoginAsync(TxtUsername.Text, TxtPassword.Password);
            if (!loginResponse.Success)
            {
                TxtStatus.Text = loginResponse.ErrorMessage ?? "Login failed.";
                Client.Dispose();
                Client = null;
                return;
            }

            Settings.ServerHost = TxtServer.Text;
            Settings.ServerPort = port;
            Settings.LastUsername = TxtUsername.Text;
            Settings.Save();

            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            TxtStatus.Text = $"Connection failed: {ex.Message}";
            Client?.Dispose();
            Client = null;
        }
        finally
        {
            BtnLogin.IsEnabled = true;
            BtnRegister.IsEnabled = true;
        }
    }
}
