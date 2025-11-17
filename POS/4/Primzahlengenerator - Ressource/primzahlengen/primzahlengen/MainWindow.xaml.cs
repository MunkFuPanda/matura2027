using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace primzahlengen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int max;
        private int foundPrim = 0;
        Storyboard r;
        int running = 0;

        public MainWindow()
        {
            InitializeComponent();
            image1.Dispatcher.Invoke(() => image1.Visibility = Visibility.Hidden);
            r = (Storyboard)FindResource("loadingRotation");
        }

        private void singleT_Prim()
        {
            lock (this) {
                running++;
            }

            List<int> prims = new List<int>();
            int i = 5;
            int tests = 0;
            int number = 0;
            prims.Add(2);
            prims.Add(3);
            while (i < max)
            {
                int maxTeiler = (int)Math.Sqrt(i) + 1;
                int j = 0;
                while (true)
                {
                    int n = prims[j];
                    int rest = (i % n);
                    ++tests;
                    if (rest == 0)
                        break; //keine Primzahl
                    if (n >= maxTeiler)
                    {
                        prims.Add(i);
                        ListView.Dispatcher.Invoke(() => ListView.Items.Add(i.ToString()));
                        break;
                    }
                    ++j;
                }
                i += 2;
            }
            number = prims.Count;
            foundPrim = prims[number - 1];

            if (Interlocked.Decrement(ref running) == 0)
            {
                r.Stop(this);
                image1.Dispatcher.Invoke(() => image1.Visibility = Visibility.Hidden);

                InputTextBox.Dispatcher.Invoke(() => InputTextBox.Text = "");
                foundPrim = 0;
            }

            
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Dispatcher.Invoke(() => ResultTextBlock.Text = "");

            try
            {
                
                image1.Dispatcher.Invoke(() => image1.Visibility = Visibility.Visible);

                max = int.Parse(InputTextBox.Text);

                if (running == 0) {
                    r.Begin(this, true);
                }

                

                ThreadPool.QueueUserWorkItem(_ => singleT_Prim());

                

                
                

                

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message);
            }
        }
    }
}