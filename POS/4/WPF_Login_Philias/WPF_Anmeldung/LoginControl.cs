using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Anmeldung
{
    public class LoginControl : Control
    {
        private TextBox _userBox;
        private PasswordBox _passwordBox;
        private Button _loginButton;
        private Button _switchtoRegistration;
        private TextBlock _errorText;
        static LoginControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoginControl), new FrameworkPropertyMetadata(typeof(LoginControl)));
        }

        public string UserInput
        {
            get => (string)GetValue(UserInputProperty);
            set => SetValue(UserInputProperty, value);
        }

        public static readonly DependencyProperty UserInputProperty =
            DependencyProperty.Register(
                nameof(UserInput),
                typeof(string),
                typeof(LoginControl),
                new PropertyMetadata(string.Empty));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(LoginControl));

        //TRUE = E-Mail | FALSE = Benutzername
        public bool LoginWithEmail
        {
            get => (bool)GetValue(LoginWithEmailProperty);
            set => SetValue(LoginWithEmailProperty, value);
        }

        public static readonly DependencyProperty LoginWithEmailProperty =
            DependencyProperty.Register(
                nameof(LoginWithEmail),
                typeof(bool),
                typeof(LoginControl),
                new PropertyMetadata(true));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _userBox = GetTemplateChild("PART_UserInput") as TextBox;
            _passwordBox = GetTemplateChild("PART_Password") as PasswordBox;
            _loginButton = GetTemplateChild("PART_LoginButton") as Button;
            _errorText = GetTemplateChild("PART_Error") as TextBlock;
            _switchtoRegistration = GetTemplateChild("PART_SwitchToRegistration") as Button;

            if (_userBox != null)
            {
                _userBox.TextChanged += (s, e) =>
                {
                    UserInput = _userBox.Text;
                };
            }

            if (_passwordBox != null)
            {
                _passwordBox.PasswordChanged += (s, e) =>
                {
                    Password = _passwordBox.Password;
                };
            }

            if (_loginButton != null)
            {
                _loginButton.Click += (s, e) =>
                {
                    LoginButton_Click(s, e); // Call the correct handler
                };
            }

            if (_switchtoRegistration != null)
            {
                _switchtoRegistration.Click += (s, e) =>
                {
                    SwitchtoRegistration_Click(s, e);

                };
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(UserInput))
            {
                ShowError("Bitte Benutzername oder E-Mail eingeben.");
                return false;
            }

            if (LoginWithEmail && !IsValidEmail(UserInput))
            {
                ShowError("Ungültige E-Mail-Adresse.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ShowError("Bitte Passwort eingeben.");
                return false;
            }

            HideError();
            return true;
        }

        private bool IsValidEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private void ShowError(string message)
        {
            if (_errorText != null)
            {
                _errorText.Visibility = Visibility.Visible;
            }
        }

        private void HideError()
        {
            if (_errorText != null)
                _errorText.Visibility = Visibility.Hidden;
        }

        public event EventHandler<LoginEventArgs> LoginRequested;
        public class LoginEventArgs : EventArgs
        {
            public string UserInput { get; set; }
            public string Password { get; set; }
            public bool LoginWithEmail { get; set; }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            LoginRequested?.Invoke(this, new LoginEventArgs
            {
                UserInput = UserInput,
                Password = Password,
                LoginWithEmail = LoginWithEmail
            });
        }

        public event EventHandler<SwitchtoRegistrationEventArgs> SwitchtoRegistration;
        public class SwitchtoRegistrationEventArgs : EventArgs
        {
        }

        private void SwitchtoRegistration_Click(object sender, RoutedEventArgs e)
        {
            SwitchtoRegistration?.Invoke(this, new SwitchtoRegistrationEventArgs
            {
            });
        }

    }
}
