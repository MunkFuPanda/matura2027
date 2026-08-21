using DataModels;
using LinqToDB;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Waldwunder_Verwaltung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public enum Bundesland
    {
        Wien,
        Niederösterreich,
        Oberösterreich,
        Salzburg,
        Tirol,
        Vorarlberg,
        Kärnten,
        Steiermark,
        Burgenland
    }

    public partial class MainWindow : Window
    {
        private WaldwunderDB db;


        private List<Waldwunder> current_waldwunder = new List<Waldwunder>();

        public MainWindow()
        {






            // ON RESIZE einbauen damit die Punkte neu gezeichnet werden
            // Karte soll auch richtig skalieren? angabe nochmal anschauen
            // Klicken auf waldwunder auf karte und fotos anzeigen funktioniert noch nicht






            InitializeComponent();

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/map.png"));
            canvas_map.Background = imageBrush;

            listwaldwunder.Visibility = Visibility.Collapsed;

            db = new WaldwunderDB(new DataOptions().UseSQLite(@"Data Source=Model\Waldwunder.db"));

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddWaldwunderDialog addWaldwunderDialog = new AddWaldwunderDialog();
            addWaldwunderDialog.ShowDialog();

            if (addWaldwunderDialog.success == true)
            {
                Waldwunder waldwunder = new Waldwunder();
                waldwunder.Id = addWaldwunderDialog.Id;
                waldwunder.Name = addWaldwunderDialog.Name;
                waldwunder.Description = addWaldwunderDialog.Beschreibung;
                waldwunder.Province = addWaldwunderDialog.Bundesland.ToString();
                waldwunder.Longitude = addWaldwunderDialog.Longitude;
                waldwunder.Latitude = addWaldwunderDialog.Latitude;
                waldwunder.Type = addWaldwunderDialog.Art;
                waldwunder.Votes = 0;

                db.Insert(waldwunder);

                foreach (String image in addWaldwunderDialog.Images)
                {
                    Bilder bild = new Bilder();
                    bild.Name = image;
                    bild.Wonder = addWaldwunderDialog.Id;

                    db.Insert(bild);
                }



            }

            
            
        }

        // STICHWORT
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            current_waldwunder.Clear();

            if (String.IsNullOrEmpty(searchBox1.Text))
            {
                MessageBox.Show("Bitte Text eingeben", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            listwaldwunder.ItemsSource = null;
            listwaldwunder.Items.Clear();
            canvas_map.Children.Clear();

            List<Waldwunder> found = db.Waldwunders.Where(x => x.Name.Contains(searchBox1.Text) || x.Description.Contains(searchBox1.Text)).ToList();

            listwaldwunder.ItemsSource = found;
            listwaldwunder.Visibility = Visibility.Visible;

            current_waldwunder = found;

            DrawPoint(found);
        }

        // ART
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            current_waldwunder.Clear();

            if (String.IsNullOrEmpty(searchBox1.Text))
            {
                MessageBox.Show("Bitte Text eingeben", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            listwaldwunder.ItemsSource = null;
            listwaldwunder.Items.Clear();
            canvas_map.Children.Clear();

            List<Waldwunder> found = db.Waldwunders.Where(x => x.Type == searchBox1.Text).ToList();

            listwaldwunder.ItemsSource = found;
            listwaldwunder.Visibility = Visibility.Visible;

            current_waldwunder = found;

            DrawPoint(found);
        }

        // ORT Longitude Latitude
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            current_waldwunder.Clear();

            if (String.IsNullOrEmpty(searchBox1.Text) && String.IsNullOrEmpty(searchBox2.Text))
            {
                MessageBox.Show("Bitte Text eingeben", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            listwaldwunder.ItemsSource = null;
            listwaldwunder.Items.Clear();
            canvas_map.Children.Clear();

            decimal inlongi = decimal.Parse(searchBox1.Text);
            decimal inlati = decimal.Parse(searchBox2.Text);
            decimal range = decimal.Parse("0.5");

            List<Waldwunder> found = db.Waldwunders.Where(x => x.Longitude >= (inlongi - range) ||
                                                          x.Longitude <= (inlongi + range) ||
                                                          x.Latitude >= (inlati - range) ||
                                                          x.Latitude <= (inlati + range)).ToList();

            listwaldwunder.ItemsSource = found;
            listwaldwunder.Visibility = Visibility.Visible;

            current_waldwunder = found;

            DrawPoint(found);
        }



        private void DrawPoint(List<Waldwunder> wonders)
        {

            canvas_map.Children.Clear();

            foreach(Waldwunder w in wonders)
            {
                double minLongitude = 9.362383;
                double maxLongitude = 17.231941;

                double minLatitude = 46.308597;
                double maxLatitude = 49.063175;

                double inlongitude = double.Parse(w.Longitude.ToString());
                double inlatitude = double.Parse(w.Latitude.ToString());

                double x = (inlongitude - minLongitude) / (maxLongitude - minLongitude) * canvas_map.ActualWidth;

                double y = (inlatitude - minLatitude) / (maxLatitude - minLatitude) * canvas_map.ActualHeight;

                Ellipse point = new Ellipse();
                point.Width = 10;
                point.Height = 10;
                point.Fill = Brushes.Red;

                Canvas.SetLeft(point, x - 5);
                Canvas.SetTop(point, y - 5);

                canvas_map.Children.Add(point);
            }


            
        }


        // ANZEIGEN
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Waldwunder sel = (Waldwunder)listwaldwunder.SelectedItem;

            List<Bilder> pictures = db.Bilders.Where(x => x.Wonder == sel.Id).ToList();

            UniformGrid grid = new UniformGrid();
            grid.Columns = 1;

            Label lname = new Label();
            lname.Content = sel.Name;
            grid.Children.Add(lname);

            Label ldesc = new Label();
            ldesc.Content = sel.Description;
            grid.Children.Add(ldesc);

            Label lprov = new Label();
            lprov.Content = sel.Province;
            grid.Children.Add(lprov);

            Label llat = new Label();
            llat.Content = sel.Latitude;
            grid.Children.Add(llat);

            Label llong = new Label();
            llong.Content = sel.Longitude;
            grid.Children.Add(llong);

            Label ltype = new Label();
            ltype.Content = sel.Type;
            grid.Children.Add(ltype);

            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");

            foreach (Bilder b in pictures)
            {
                Image img = new Image();

                string combinedpath = System.IO.Path.Combine(path, b.Name);

                img.Source = new BitmapImage(new Uri(combinedpath, UriKind.RelativeOrAbsolute));
                grid.Children.Add(img);
            }

            ShowWaldwunderDialog showWaldwunderDialog = new ShowWaldwunderDialog(grid);
            showWaldwunderDialog.ShowDialog();



        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (current_waldwunder.Count == 0)
            {
                return;
            }

            DrawPoint(current_waldwunder);
        }

        /*private void UpdateMapPins(List<Waldwunder> wonders)
        {
            canvas_map.Children.Clear();

            foreach (var w in wonders)
            {
                if (w.Latitude == null || w.Longitude == null) continue;

                // Umrechnung von decimal? auf double für WPF Canvas-Positionierung
                double lat = (double)w.Latitude.Value;
                double lon = (double)w.Longitude.Value;

                // Prozentuale Position berechnen (Dreisatz)
                double xPct = (lon - MapLonLeft) / (MapLonRight - MapLonLeft);
                double yPct = (MapLatTop - lat) / (MapLatTop - MapLatBottom); // Oben ist Lat höher, deshalb invertiert

                double xPos = xPct * MapCanvas.ActualWidth;
                double yPos = yPct * MapCanvas.ActualHeight;

                // Pin als kleiner Kreis
                Ellipse pin = new Ellipse
                {
                    Width = 14,
                    Height = 14,
                    Fill = Brushes.Crimson,
                    Stroke = Brushes.White,
                    StrokeThickness = 1.5,
                    Tag = w,
                    ToolTip = w.Name
                };

                pin.MouseDown += Pin_MouseDown;

                // Zentrieren des Pins auf dem Koordinatenpunkt
                Canvas.SetLeft(pin, xPos - 7);
                Canvas.SetTop(pin, yPos - 7);
                MapCanvas.Children.Add(pin);
            }
        }

        private void Pin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse pin && pin.Tag is Waldwunder w)
            {
                // Synchronisiere die Auswahl mit der ListBox anhand der ID
                var item = WonderListBox.Items.Cast<Waldwunder>().FirstOrDefault(i => i.Id == w.Id);
                if (item != null)
                {
                    WonderListBox.SelectedItem = item;
                }
            }
        }

        private void MapCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Verhindert das Verschwinden der Pins beim Ändern der Fenstergröße
            if (WonderListBox.ItemsSource is List<Waldwunder> currentList)
            {
                UpdateMapPins(currentList);
            }
        }
        */
    }
}