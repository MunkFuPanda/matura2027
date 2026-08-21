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
using WPF_Login_Registration;

namespace WPF_Login_Registration_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RegistrationControl.Visibility = Visibility.Hidden;
        }

        private void Login_SwitchToRegistration(object sender, RoutedEventArgs e)
        {
            LoginControl.Visibility = Visibility.Hidden;
            RegistrationControl.Visibility = Visibility.Visible;
        }

        private void Login_LLogin(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(LoginControl.Email + " " + LoginControl.Password);
        }
    }
} 