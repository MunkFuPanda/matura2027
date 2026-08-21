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
using DataModels;
using LinqToDB;

namespace dbtestfor4thpa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private WaldwunderDB db;
        public MainWindow()
        {
            InitializeComponent();

            db = new WaldwunderDB(new DataOptions().UseSQLite(@"Data Source=Model\Waldwunder.db"));

            MessageBox.Show(db.Waldwunders.First().Name.ToString());


        }
    }
}