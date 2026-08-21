using System.Collections;
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

namespace Drei_Raucher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Thread> smoker_agent_threads = new();

        public MainWindow()
        {
            InitializeComponent();
        }


        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            int smoketime1 = int.Parse(tb_raucher_1.Text);
            int smoketime2 = int.Parse(tb_raucher_2.Text);
            int smoketime3 = int.Parse(tb_raucher_3.Text);

            Smoker_Agent r1 = new Smoker_Agent(1, smoketime1, Zutaten.Tabak, new List<Zutaten> { Zutaten.Zigarettenpapier, Zutaten.Streichholz }, lb_table_1, lb_table_2, lb_status_1, lb_need_1, Zutaten.Zigarettenpapier, Zutaten.Streichholz);
            Smoker_Agent r2 = new Smoker_Agent(2, smoketime2, Zutaten.Zigarettenpapier, new List<Zutaten> { Zutaten.Tabak, Zutaten.Streichholz }, lb_table_1, lb_table_2, lb_status_2, lb_need_2, Zutaten.Tabak, Zutaten.Streichholz);
            Smoker_Agent r3 = new Smoker_Agent(3, smoketime3, Zutaten.Streichholz, new List<Zutaten> { Zutaten.Zigarettenpapier, Zutaten.Tabak }, lb_table_1, lb_table_2, lb_status_3, lb_need_3, Zutaten.Zigarettenpapier, Zutaten.Tabak);
            Smoker_Agent a1 = new Smoker_Agent(4, smoketime1, Zutaten.Tabak, new List<Zutaten> { Zutaten.Zigarettenpapier, Zutaten.Streichholz }, lb_table_1, lb_table_2, null, null, Zutaten.Zigarettenpapier, Zutaten.Tabak);

            Thread ta1 = new Thread(new ThreadStart(a1.Agent));
            Thread tr1 = new Thread(new ThreadStart(r1.Smoker));
            Thread tr2 = new Thread(new ThreadStart(r2.Smoker));
            Thread tr3 = new Thread(new ThreadStart(r3.Smoker));

            ta1.Start();
            tr1.Start();
            tr2.Start();
            tr3.Start();

            smoker_agent_threads.Add(ta1);
            smoker_agent_threads.Add(tr1);
            smoker_agent_threads.Add(tr2);
            smoker_agent_threads.Add(tr3);

        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var t in  smoker_agent_threads)
            {
                t.Interrupt();
                t.Join();
            }

            lb_need_1.Content = "";
            lb_need_2.Content = "";
            lb_need_3.Content = "";

            lb_table_1.Content = "";
            lb_table_2.Content = "";

            lb_status_1.Content = "";
            lb_status_2.Content = "";
            lb_status_3.Content = "";
        }
    }
}