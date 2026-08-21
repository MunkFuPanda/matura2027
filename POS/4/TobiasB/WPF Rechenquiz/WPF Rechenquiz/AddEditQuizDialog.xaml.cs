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

namespace WPF_Rechenquiz
{
    /// <summary>
    /// Interaktionslogik für AddEditQuiz.xaml
    /// </summary>
    public partial class AddEditQuizDialog : Window
    {
        List<Calculation> calculations = new List<Calculation>();
        public AddEditQuizDialog(bool addNew)
        {
            InitializeComponent();

            calculations.Add(new Calculation(1, 2, ArithOperators.Add));
            calculations.Add(new Calculation());

            if (addNew)
            {
                lb_calcs.ItemsSource = calculations;
            }
            else
            {

            }
        }

        private void lb_calcs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
