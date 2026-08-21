using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Login_Registration
{
    
    public class Login : Control
    {
        static Login()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Login), new FrameworkPropertyMetadata(typeof(Login)));
        }


        private string email = "";
        private string password = "";

        // Dependency Properties

        //public static readonly DependencyProperty
        //    EmailProperty = DependencyProperty.Register(
        //        "Email",
        //        typeof(string),
        //        typeof(Login),
        //        new FrameworkPropertyMetadata(
        //            "", null));

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
            }
        }

        //public static readonly DependencyProperty
        //    PasswordProperty = DependencyProperty.Register(
        //        "Password",
        //        typeof(string),
        //        typeof(Login),
        //        new FrameworkPropertyMetadata("", null));

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
            }
        }



        public static readonly DependencyProperty
            UsernameOrEmailProperty = DependencyProperty.Register(
                "UsernameOrEmail",
                typeof(bool),
                typeof(Login),
                new FrameworkPropertyMetadata(false, OnUsernameOrEmailChanged));

        public bool UsernameOrEmail
        {
            get { return (bool)base.GetValue(UsernameOrEmailProperty); }
            set
            {
                base.SetValue(UsernameOrEmailProperty, value);
            }
        }

        private static void OnUsernameOrEmailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Login)d;

            bool newValue = (bool)e.NewValue;
            control.ReactUsernameOrEmailChanged(newValue);
        }

        private protected virtual void ReactUsernameOrEmailChanged(bool newValue)
        {
            UsernameOrEmail = newValue;

            if (UsernameOrEmail == false)
            {
                ChangeUserNameOrEmail("Email");
            }
            else
            {
                ChangeUserNameOrEmail("Username");
            }
        }

        // variables

        Label EmailOrUsername;


        private void ChangeUserNameOrEmail(string changeto)
        {
            if (EmailOrUsername == null)
            {
                return;
            }


            EmailOrUsername.Content = changeto;
        }


        // Routed Events

        public static readonly RoutedEvent SwitchToRegistrationEvent =
            EventManager.RegisterRoutedEvent("SwitchToRegistration",
                RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                typeof(Login));

        public event RoutedEventHandler SwitchToRegistration
        {
            add { base.AddHandler(SwitchToRegistrationEvent, value); }
            remove { base.RemoveHandler(SwitchToRegistrationEvent, value); }
        }

        protected void FireSwitchToRegistration()
        {
            base.RaiseEvent(new RoutedEventArgs(SwitchToRegistrationEvent));
        }

        public static readonly RoutedEvent LoginEvent =
            EventManager.RegisterRoutedEvent("LLogin",
                RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                typeof(Login));

        public event RoutedEventHandler LLogin
        {
            add { base.AddHandler(LoginEvent, value); }
            remove { base.RemoveHandler(LoginEvent, value); }
        }

        protected void FireLogin()
        {
            base.RaiseEvent(new RoutedEventArgs(LoginEvent));
        }


        void OnLoginButton(object sender, RoutedEventArgs e)
        {
            TextBox tbemailuser = (TextBox)this.Template.FindName("PART_TB_EMAIL", this);
            PasswordBox pbpassword = (PasswordBox)this.Template.FindName("PART_TB_PASSWORD", this);

            TextBlock tberror = (TextBlock)this.Template.FindName("PART_TB_ERROR", this);

            if (UsernameOrEmail == true)
            {
                if (tbemailuser.Text == "")
                {
                    tberror.Text = "Please fill in username";
                    tberror.Visibility = Visibility.Visible;
                    tbemailuser.Focus();
                    return;
                }
            }
            else
            {
                if (tbemailuser.Text == "")
                {
                    tberror.Text = "Please fill in email";
                    tberror.Visibility = Visibility.Visible;
                    tbemailuser.Focus();
                    return;
                }

                if (!Regex.IsMatch(tbemailuser.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                {
                    tberror.Text = "Please enter valid email";
                    tberror.Visibility = Visibility.Visible;
                    tbemailuser.Focus();
                    return;
                }

            }

            if (pbpassword.Password == "")
            {
                tberror.Text = "Please fill in password";
                tberror.Visibility = Visibility.Visible;
                pbpassword.Focus();
                return;
            }

            tberror.Visibility = Visibility.Hidden;

            email = tbemailuser.Text;
            password = pbpassword.Password;

            FireLogin();
        }


        void OnToRegistrationButton(object sender, RoutedEventArgs e)
        {
            FireSwitchToRegistration();
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Set button click function

            Button bLogin = (Button)this.Template.FindName("PART_LOGIN_BUTTON", this);
            bLogin.Click += OnLoginButton;

            Button bToRegistration = (Button)this.Template.FindName("PART_TO_REGISTRATION", this);
            bToRegistration.Click += OnToRegistrationButton;

            // set Label for Username or Email

            EmailOrUsername = (Label)this.Template.FindName("PART_L_EMAIL", this);

            if (UsernameOrEmail == false)
            {
                ChangeUserNameOrEmail("Email");
            }
            else
            {
                ChangeUserNameOrEmail("Username");
            }


            // Bindings

            //TextBox tbEmail = (TextBox)this.Template.FindName("PART_TB_EMAIL", this);
            //Binding bindingEmail = new Binding();
            //bindingEmail.Source = this;
            //bindingEmail.Path = new PropertyPath("Email");
            //tbEmail.SetBinding(TextBox.TextProperty, bindingEmail);

            //PasswordBox tbPassword = (PasswordBox)this.Template.FindName("PART_TB_PASSWORD", this);
            //Binding bindingPassword = new Binding();
            //bindingPassword.Source = this;
            //bindingPassword.Path = new PropertyPath("Password");
            //tbPassword.SetBinding(PasswordBox., bindingPassword);
        }
    }
}