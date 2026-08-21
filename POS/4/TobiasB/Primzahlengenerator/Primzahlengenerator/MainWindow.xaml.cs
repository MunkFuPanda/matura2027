using System.Configuration;
using System.Text;
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
using static System.Net.Mime.MediaTypeNames;

namespace Primzahlengenerator
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

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Prim));
        }

        private void Prim(object o)
        {
            Storyboard r = new Storyboard();
            load_image.Dispatcher.Invoke(() =>
            {
                r = (Storyboard)FindResource("loadingRotation");
                load_image.Visibility = Visibility.Visible;
                r.Begin(this, true);
            });

            Thread.Sleep(1000);


            Int128 max = 0;
            input.Dispatcher.Invoke(() =>
            {
                max = Int128.Parse(input.Text);
            });
            


            List<int> prims = new List<int>();
            prims.Add(2);
            prims.Add(3);

            lv_prim.Dispatcher.Invoke(() =>
            {
                lv_prim.Items.Add(2);
                lv_prim.Items.Add(3);
            });

            int i = 5;
            while (i < max)
            {
                int maxTeiler = (int)Math.Sqrt(i) + 1;
                int j = 0;
                while (true)
                {
                    int n = prims[j];
                    int rest = (i % n);
                    if (rest == 0)
                        break; //keine Primzahl
                    if (n >= maxTeiler)
                    {
                        prims.Add(i);

                        lv_prim.Dispatcher.Invoke(() =>
                        {
                            lv_prim.Items.Add(i);
                        });

                        break;
                    }
                    ++j;
                }
                i += 2;
            }
            int number = prims.Count;
            int maxPrim = prims[number - 1];

            output.Dispatcher.Invoke(new Action(() =>
            {
                output.Content = maxPrim;
            }));

            Thread.Sleep(1000);

            load_image.Dispatcher.Invoke(new Action(() =>
            {
                r.Stop(this);
                load_image.Visibility = Visibility.Hidden;
            }));

        }
    }
}