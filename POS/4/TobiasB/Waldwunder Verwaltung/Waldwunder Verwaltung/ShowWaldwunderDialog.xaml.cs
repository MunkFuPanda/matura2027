using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataModels;

namespace Waldwunder_Verwaltung
{
    /// <summary>
    /// Interaktionslogik für ShowWaldwunderDialog.xaml
    /// </summary>
    public partial class ShowWaldwunderDialog : Window
    {
        public ShowWaldwunderDialog(UniformGrid ug)
        {
            InitializeComponent();
            Content = ug;
        }
    }
}
