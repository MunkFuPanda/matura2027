using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_Chat_Client
{
    /// <summary>
    /// Interaktionslogik für LoginDialog.xaml
    /// </summary>
    public partial class RegistrationDialog : Window
    {
        public String UserName { get => username; set { username = value; } }
        public String Password { get => password; set { password = value; } }
        public bool Ok { get => ok; set { ok = value; } }

        private String username;
        private String password;
        private bool ok = false;
        public RegistrationDialog()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(usernameTextBox.Text))
            {
                MessageBox.Show("Bitte Username eingeben", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                ok = false;
                return;
            }
            if (String.IsNullOrEmpty(passwordBox.Password))
            {
                MessageBox.Show("Bitte Passwort eingeben", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                ok = false;
                return;
            }

            username = usernameTextBox.Text;
            password = passwordBox.Password;
            ok = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ok = false;
            Close();
        }
    }
}
