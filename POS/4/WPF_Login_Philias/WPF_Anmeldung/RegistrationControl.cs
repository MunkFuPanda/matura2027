using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WPF_Anmeldung
{
    public class RegistrationControl : Control
    {
        // ================= TEMPLATE PARTS =================
        private TextBox _vnameBox;
        private TextBox _nnameBox;
        private TextBox _emailBox;
        private TextBox _addressBox;
        private PasswordBox _passwordBox;
        private PasswordBox _cpasswordBox;
        private Button _submitButton;
        private Button _resetButton;
        private Button _cancelButton;
        private Button _switchToLoginButton;
        private TextBlock _errorText;

        static RegistrationControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RegistrationControl),
                new FrameworkPropertyMetadata(typeof(RegistrationControl)));
        }

        // ================= DEPENDENCY PROPERTIES =================

        public string VName
        {
            get => (string)GetValue(VNameProperty);
            set => SetValue(VNameProperty, value);
        }

        public static readonly DependencyProperty VNameProperty =
            DependencyProperty.Register(nameof(VName), typeof(string), typeof(RegistrationControl));

        public string NName
        {
            get => (string)GetValue(NNameProperty);
            set => SetValue(NNameProperty, value);
        }

        public static readonly DependencyProperty NNameProperty =
            DependencyProperty.Register(nameof(NName), typeof(string), typeof(RegistrationControl));

        public string Email
        {
            get => (string)GetValue(EmailProperty);
            set => SetValue(EmailProperty, value);
        }

        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register(nameof(Email), typeof(string), typeof(RegistrationControl));

        public string Address
        {
            get => (string)GetValue(AddressProperty);
            set => SetValue(AddressProperty, value);
        }

        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register(nameof(Address), typeof(string), typeof(RegistrationControl));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(RegistrationControl));

        public string ConfirmPassword
        {
            get => (string)GetValue(ConfirmPasswordProperty);
            set => SetValue(ConfirmPasswordProperty, value);
        }

        public static readonly DependencyProperty ConfirmPasswordProperty =
            DependencyProperty.Register(nameof(ConfirmPassword), typeof(string), typeof(RegistrationControl));

        // ================= ROUTED EVENTS =================

        public static readonly RoutedEvent RegisterEvent =
            EventManager.RegisterRoutedEvent(
                nameof(Register),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(RegistrationControl));

        public event RoutedEventHandler Register
        {
            add => AddHandler(RegisterEvent, value);
            remove => RemoveHandler(RegisterEvent, value);
        }

        public static readonly RoutedEvent CancelEvent =
            EventManager.RegisterRoutedEvent(
                nameof(Cancel),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(RegistrationControl));

        public event RoutedEventHandler Cancel
        {
            add => AddHandler(CancelEvent, value);
            remove => RemoveHandler(CancelEvent, value);
        }

        public static readonly RoutedEvent SwitchToLoginEvent =
            EventManager.RegisterRoutedEvent(
                nameof(SwitchToLogin),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(RegistrationControl));

        public event RoutedEventHandler SwitchToLogin
        {
            add => AddHandler(SwitchToLoginEvent, value);
            remove => RemoveHandler(SwitchToLoginEvent, value);
        }

        // ================= TEMPLATE HOOK =================

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _vnameBox = GetTemplateChild("PART_VNAME") as TextBox;
            _nnameBox = GetTemplateChild("PART_NNAME") as TextBox;
            _emailBox = GetTemplateChild("PART_EMAIL") as TextBox;
            _addressBox = GetTemplateChild("PART_ADDRESS") as TextBox;
            _passwordBox = GetTemplateChild("PART_PASSWORD") as PasswordBox;
            _cpasswordBox = GetTemplateChild("PART_CPASSWORD") as PasswordBox;
            _submitButton = GetTemplateChild("PART_SUBMIT") as Button;
            _resetButton = GetTemplateChild("PART_RESET") as Button;
            _cancelButton = GetTemplateChild("PART_CANCEL") as Button;
            _switchToLoginButton = GetTemplateChild("PART_switchToLogin") as Button;
            _errorText = GetTemplateChild("PART_Error") as TextBlock;

            if (_vnameBox != null)
                _vnameBox.TextChanged += (_, __) => VName = _vnameBox.Text;

            if (_nnameBox != null)
                _nnameBox.TextChanged += (_, __) => NName = _nnameBox.Text;

            if (_emailBox != null)
                _emailBox.TextChanged += (_, __) => Email = _emailBox.Text;

            if (_addressBox != null)
                _addressBox.TextChanged += (_, __) => Address = _addressBox.Text;

            if (_passwordBox != null)
                _passwordBox.PasswordChanged += (_, __) => Password = _passwordBox.Password;

            if (_cpasswordBox != null)
                _cpasswordBox.PasswordChanged += (_, __) => ConfirmPassword = _cpasswordBox.Password;

            if (_submitButton != null)
                _submitButton.Click += (_, __) => Register_Click();

            if (_resetButton != null)
                _submitButton.Click += (_, __) => Reset_Click();

            if (_cancelButton != null)
                _submitButton.Click += (_, __) => Cancel_Click();

            if (_switchToLoginButton != null)
                _switchToLoginButton.Click += (_, __) =>
                    RaiseEvent(new RoutedEventArgs(SwitchToLoginEvent));
        }

        // ================= VALIDATION =================

        private void Register_Click()
        {
            if (!Validate())
                return;

            RaiseEvent(new RoutedEventArgs(RegisterEvent));
        }
        private void Cancel_Click()
        {
            RaiseEvent(new RoutedEventArgs(CancelEvent));
        }

        private void Reset_Click()
        {
            _vnameBox.Text = "";
            _nnameBox.Text = "";
            _emailBox.Text = "";
            _addressBox.Text = "";
            _passwordBox.Password = "";
            _cpasswordBox.Password = "";
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(VName) ||
                string.IsNullOrWhiteSpace(NName))
            {
                ShowError("Vor- und Nachname sind erforderlich.");
                return false;
            }

            if (!IsValidEmail(Email))
            {
                ShowError("Ungültige E-Mail-Adresse.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ShowError("Bitte Passwort und Bestätigung eingeben.");
                return false;
            }

            if (Password != ConfirmPassword)
            {
                ShowError("Passwörter stimmen nicht überein.");
                return false;
            }

            HideError();
            return true;
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email ?? "",
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private void ShowError(string message)
        {
            if (_errorText != null)
            {
                _errorText.Text = message;
                _errorText.Visibility = Visibility.Visible;
            }
        }

        private void HideError()
        {
            if (_errorText != null)
                _errorText.Visibility = Visibility.Collapsed;
        }
    }
}
