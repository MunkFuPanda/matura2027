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
using WPF_Rating_System_Lib;

namespace WPF_Rating_System_Impl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty
        SliderValueProperty = DependencyProperty.Register(
            "SliderValue",
            typeof(double),
            typeof(RatingSystem),
            new PropertyMetadata(0d, null));

        public double SliderValue
        {
            get { return (double)base.GetValue(SliderValueProperty); }
            set { base.SetValue(SliderValueProperty, value); }
        }
    }
}