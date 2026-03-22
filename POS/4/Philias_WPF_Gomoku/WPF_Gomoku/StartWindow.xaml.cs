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
using System.Windows.Shapes;

namespace WPF_Gomoku
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public int GameMode { get; private set; } = 1; // 1: PVP, 2: PVC, 3: Network Host, 4: Network Join
        public string IPAddress { get; private set; } = "127.0.0.1";
        public int Port { get; private set; } = 12345;
        public int size { get; private set; } = 9;
        public StartWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameMode = rb1.IsChecked == true ? 1 :
                       rb2.IsChecked == true ? 2 :
                       rb3.IsChecked == true ? 3 : 4;
            IPAddress = tb1.Text;
            if (!int.TryParse(tb2.Text, out int port))
            {
                MessageBox.Show("Invalid port number. Please enter a valid integer.");
                return;
            }
            Port = port;
            
            switch (cb1.SelectedIndex)
            {
                case 0:
                    size = 9;
                    break;
                case 1:
                    size = 12;
                    break;
                case 2:
                    size = 15;
                    break;
                default:
                    size = 9;
                    break;
            }

            DialogResult = true;
            Close();
        }

        private void GameMode_Changed(object sender, RoutedEventArgs e)
        {
            if (rb3.IsChecked == true || rb4.IsChecked == true)
            {
                NetworkGrid.Visibility = Visibility.Visible;
                cb1.Items.Clear();

                if (rb3.IsChecked == true)
                {
                    cb1.Items.Add("9x9");
                    cb1.Items.Add("12x12");
                    cb1.Items.Add("15x15");
                }
            }

            else if (rb2.IsChecked == true) {
                cb1.Items.Clear();
                cb1.Items.Add("15x15");
            }

            else
            {
                NetworkGrid.Visibility = Visibility.Collapsed;
                cb1.Items.Clear();
                cb1.Items.Add("9x9");
                cb1.Items.Add("12x12");
                cb1.Items.Add("15x15");
            }
        }
    }
}
