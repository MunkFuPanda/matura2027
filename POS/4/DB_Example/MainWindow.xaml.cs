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
using DataModel;
using LinqToDB;
using LinqToDB.Data;

namespace DB_Example {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            MyDB dB = new MyDB(new DataOptions().UseSQLite(@"Data Source=Database/db.sqlite"));

            dB.Users.ToList().ForEach(user => {
                MessageBox.Show($"User: {user.Fname} {user.Lname}");
            });
        }
    }
}