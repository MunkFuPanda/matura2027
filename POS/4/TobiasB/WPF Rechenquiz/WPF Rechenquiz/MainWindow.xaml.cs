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

namespace WPF_Rechenquiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Quiz> quizzes = new List<Quiz>();

        public MainWindow()
        {
            InitializeComponent();


        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {

            }
            else if (tabControl.SelectedIndex == 1)
            {
                // Dialog mit Buttons
            }
            else if (tabControl.SelectedIndex == 2)
            {

            }
            else
            {

            }
        }


        private void AddQuiz_Click(object sender, RoutedEventArgs e)
        {
            // true = neues Quiz erstellen
            AddEditQuizDialog addEditQuizDialog = new AddEditQuizDialog(true);
            addEditQuizDialog.ShowDialog();
        }

        private void EditQuiz_Click(object sender, RoutedEventArgs e)
        {
            // false = altes Quiz bearbeiten
            AddEditQuizDialog addEditQuizDialog = new AddEditQuizDialog(false);
            addEditQuizDialog.ShowDialog();
        }
    }
}