using DataModels;
using LinqToDB;
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

namespace u_waldwunder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Waldwunder> allWaldwunder = new List<Waldwunder>();
        List<Waldwunder> filteredWaldwunder = new List<Waldwunder>();
        List<Bilder> allBilder = new List<Bilder>();

        double lat_max = 49.063175;
        double lat_min = 46.308597;

        double lon_max = 17.231941;
        double lon_min = 9.362383;

        public MainWindow()
        {
            InitializeComponent();

            SearchComboBox.Items.Add("Stichwort");
            SearchComboBox.Items.Add("Art");
            SearchComboBox.Items.Add("Ort");

            using var db = new WaldwunderDB(
                new DataOptions().UseSQLite($"Data Source={System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db", "Waldwunder.db")}")
            );

            allWaldwunder = db.Waldwunders.ToList();
            allBilder = db.Bilders.ToList();
        }

        private void searchButtonClick(object sender, RoutedEventArgs e)
        {
            int selectedIndex = SearchComboBox.SelectedIndex;
            string searchText = SearchTextBox.Text.ToLower().Trim();

            switch (selectedIndex)
            {
                case 0: // Stichwort
                    filteredWaldwunder = allWaldwunder.Where(w => w.Name.ToLower().Contains(searchText)).ToList();
                    break;
                case 1: // Art
                    filteredWaldwunder = allWaldwunder.Where(w => w.Type.ToLower().Equals(searchText)).ToList();
                    break;
                case 2: // Ort
                    //todo
                    //filteredWaldwunder = allWaldwunder.Where(w => w.Ort.ToLower().Contains(searchText)).ToList();
                    break;
                default:
                    filteredWaldwunder = new List<Waldwunder>();
                    break;
            }
            WaldwunderBox.ItemsSource = filteredWaldwunder;
            MapCanvas.Children.Clear();
            DrawPoints();
        }

        private void DrawPoints()
        {
            double canvasWidth = MapCanvas.ActualWidth;
            double canvasHeight = MapCanvas.ActualHeight;

            foreach (var waldwunder in filteredWaldwunder)
            {
                double x = ((double)waldwunder.Longitude - lon_min) / (lon_max - lon_min) * canvasWidth;
                double y = (lat_max - (double)waldwunder.Latitude) / (lat_max - lat_min) * canvasHeight;

                Ellipse marker = new Ellipse
                {
                    Width = 12,
                    Height = 12,
                    Fill = Brushes.Red,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Tag = waldwunder.Id,
                };

                marker.MouseLeftButtonDown += new MouseButtonEventHandler((s, e) =>
                {
                    if (s is Ellipse clickedMarker && clickedMarker.Tag is long waldwunderId)
                    {
                        var selectedWaldwunder = allWaldwunder.FirstOrDefault(w => w.Id == waldwunderId);
                        if (selectedWaldwunder != null)
                        {
                            WaldwunderBox.SelectedItem = selectedWaldwunder;
                            ShowDetailsButton.IsEnabled = true;
                        }
                    }
                });

                Canvas.SetLeft(marker, x - 6);
                Canvas.SetTop(marker, y - 6);

                MapCanvas.Children.Add(marker);
            }

        }

        private void neuesWaldwunder(object sender, RoutedEventArgs e)
        {
            RegisterDialog registerDialog = new RegisterDialog();
            registerDialog.Owner = this;
            registerDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            registerDialog.ShowDialog();
        }

        private void showDetailsButtonClick(object sender, RoutedEventArgs e)
        {
            WaldwunderDetails waldwunderDetails = new WaldwunderDetails();
            if (WaldwunderBox.SelectedItem is Waldwunder selectedWaldwunder)
            {
                List<Bilder> bilderForWaldwunder = new List<Bilder>();
                bilderForWaldwunder = allBilder.Where(b => b.Wonder == selectedWaldwunder.Id).ToList();
                waldwunderDetails.SetWaldwunder(selectedWaldwunder, bilderForWaldwunder);
                waldwunderDetails.Owner = this;
                waldwunderDetails.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                waldwunderDetails.ShowDialog();
            }
        }

        private void textChangedTextbox(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text.Length > 0 && SearchComboBox.SelectedIndex >= 0)
            {
                SearchButton.IsEnabled = true;
            }
            else
            {
                SearchButton.IsEnabled = false;
            }
        }

        private void selectionChangedCombobox(object sender, SelectionChangedEventArgs e)
        {
            if (SearchTextBox.Text.Length > 0 && SearchComboBox.SelectedIndex >= 0)
            {
                SearchButton.IsEnabled = true;
            }
            else
            {
                SearchButton.IsEnabled = false;
            }
        }

        private void ChosenWaldwunderChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WaldwunderBox.SelectedItem is Waldwunder selectedWaldwunder)
            {
                ShowDetailsButton.IsEnabled = true;
            }
            else
            {
                ShowDetailsButton.IsEnabled = false;
            }
        }
    }
}