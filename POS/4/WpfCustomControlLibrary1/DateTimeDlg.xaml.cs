using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wecker {
    /// <summary>
    /// Interaction logic for DateTimeDlg.xaml
    /// </summary>
    public partial class DateTimeDlg : Window {
        DateTime alarmTime;

        public DateTime AlarmTime {
            get { return alarmTime; }
            set {
                alarmTime = value;
                this.dtp.Value = alarmTime;
            }
        }

        public DateTimeDlg() {
            InitializeComponent();
        }

        private void buttonOkay_Click(object sender, RoutedEventArgs e) {
            if (dtp.Value != null) {
                alarmTime = (DateTime)this.dtp.Value;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
            this.Close();
        }
    }
}
