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
using WPF_Anmeldung;
using static WPF_Anmeldung.LoginControl;

namespace Test_Anmeldung
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

        private void LoginControl_LoginRequested(object sender, LoginEventArgs e)
        {
            MessageBox.Show(
                $"User: {e.UserInput}\n" +
                $"Passwort: {e.Password}\n" +
                $"E-Mail Login: {e.LoginWithEmail}");
        }

        private void loginControl_SwitchtoRegistration(object sender, SwitchtoRegistrationEventArgs e)
        {
            Visibility vs1 = loginControl.Visibility;
            loginControl.Visibility = registrationControl.Visibility;
            registrationControl.Visibility = vs1;
        }

        private void registrationControl_Register(object sender, RoutedEventArgs e)
        {
            var reg = (RegistrationControl)sender;
            {
                MessageBox.Show($"Registrierung mit E-Mail: {reg.Email}");
            }
        }

        private void registrationControl_SwitchToLogin(object sender, RoutedEventArgs e)
        {
            loginControl.Visibility = Visibility.Visible;
            registrationControl.Visibility = Visibility.Collapsed;
        }

        private void registrationControl_Cancel(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Registrierung abgebrochen.");
        }
    }
}