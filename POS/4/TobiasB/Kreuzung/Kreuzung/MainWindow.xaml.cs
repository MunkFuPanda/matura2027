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

namespace Kreuzung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            crossroad_type.Items.Add("Kleine Kreuzung");
            crossroad_type.Items.Add("Mehrere Spuren");
            crossroad_type.Items.Add("Ampel");
            crossroad_type.Items.Add("Sensor-Ampel");

        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {

            // Funktion auslagern, nicht nur if else
            // GUI Uniform Grid verwenden und die GUI Elemente auf die
            // freien Bereiche aufteilen (Eingabe, Label, Ampel)
            
            if (crossroad_type.SelectedItem == "Kleine Kreuzung")
            {
                Crossroad road = new Crossroad(lb_middle);

                Dictionary<Direction, List<ListBox>> dict = new Dictionary<Direction, List<ListBox>>();

                dict.Add(Direction.South, new List<ListBox>() { lb_north, lb_south });
                dict.Add(Direction.North, new List<ListBox>() { lb_south, lb_north });
                dict.Add(Direction.East, new List<ListBox>() { lb_west, lb_east });
                dict.Add(Direction.West, new List<ListBox>() { lb_east, lb_west });

                for (int i = 0; i < int.Parse(count_car.Text); i++)
                {
                    Direction dir = (Direction)new Random().Next(0, 4);
                    List<ListBox> list = dict.GetValueOrDefault(dir);
                    Car car = new Car(i, dir, list.First(), list.Last(), road);
                    Thread tc = new Thread(new ThreadStart(car.drive));
                    tc.Start();
                }
            }

            else if (crossroad_type.SelectedItem == "Mehrere Spuren")
            {
                LargeCrossroad road = new LargeCrossroad(lb_middle);

                Dictionary<Direction, List<ListBox>> dict = new Dictionary<Direction, List<ListBox>>();

                dict.Add(Direction.South, new List<ListBox>() { lb_north, lb_south });
                dict.Add(Direction.North, new List<ListBox>() { lb_south, lb_north });
                dict.Add(Direction.East, new List<ListBox>() { lb_west, lb_east });
                dict.Add(Direction.West, new List<ListBox>() { lb_east, lb_west });

                for (int i = 0; i < int.Parse(count_car.Text); i++)
                {
                    Direction dir = (Direction)new Random().Next(0, 4);
                    List<ListBox> list = dict.GetValueOrDefault(dir);
                    Car car = new Car(i, dir, list.First(), list.Last(), road);
                    Thread tc = new Thread(new ThreadStart(car.drive));
                    tc.Start();
                }
            }

            else if (crossroad_type.SelectedItem == "Ampel")
            {
                AmpelCrossroad road = new AmpelCrossroad(lb_middle, lb_ampel);

                Dictionary<Direction, List<ListBox>> dict = new Dictionary<Direction, List<ListBox>>();

                dict.Add(Direction.South, new List<ListBox>() { lb_north, lb_south });
                dict.Add(Direction.North, new List<ListBox>() { lb_south, lb_north });
                dict.Add(Direction.East, new List<ListBox>() { lb_west, lb_east });
                dict.Add(Direction.West, new List<ListBox>() { lb_east, lb_west });

                for (int i = 0; i < int.Parse(count_car.Text); i++)
                {
                    Direction dir = (Direction)new Random().Next(0, 4);
                    List<ListBox> list = dict.GetValueOrDefault(dir);
                    Car car = new Car(i, dir, list.First(), list.Last(), road);
                    Thread tc = new Thread(new ThreadStart(car.drive));
                    tc.Start();
                }

                Thread amp = new Thread(new ThreadStart(road.Ampel));
                amp.Start();
            }

            else if (crossroad_type.SelectedItem == "Sensor-Ampel")
            {
                SensorAmpel road = new SensorAmpel(lb_middle, lb_ampel);

                Dictionary<Direction, List<ListBox>> dict = new Dictionary<Direction, List<ListBox>>();

                dict.Add(Direction.South, new List<ListBox>() { lb_north, lb_south });
                dict.Add(Direction.North, new List<ListBox>() { lb_south, lb_north });
                dict.Add(Direction.East, new List<ListBox>() { lb_west, lb_east });
                dict.Add(Direction.West, new List<ListBox>() { lb_east, lb_west });

                for (int i = 0; i < int.Parse(count_car.Text); i++)
                {
                    Direction dir = (Direction)new Random().Next(0, 4);
                    List<ListBox> list = dict.GetValueOrDefault(dir);
                    Car car = new Car(i, dir, list.First(), list.Last(), road);
                    Thread tc = new Thread(new ThreadStart(car.drive));
                    tc.Start();
                }

                Thread amp = new Thread(new ThreadStart(road.Ampel));
                amp.Start();
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            lb_east.Items.Clear();
            lb_west.Items.Clear();
            lb_south.Items.Clear();
            lb_north.Items.Clear();
            lb_middle.Items.Clear();
            lb_ampel.Content = "Ampel";
            lb_ampel.Background = Brushes.White;
        }
    }
}