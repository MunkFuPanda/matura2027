using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Waldwunder_Verwaltung
{
    /// <summary>
    /// Interaktionslogik für AddWaldwunderDialog.xaml
    /// </summary>
    public partial class AddWaldwunderDialog : Window
    {
        private List<string> images = new List<string>();
        private List<string> safeimages = new List<string>();

        public bool success = false;

        public int Id { get; set; }
        public string Name {  get; set; }
        public string Beschreibung { get; set; }
        public Bundesland Bundesland { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Art { get; set; }
        public List<string> Images { get; set; }




        public AddWaldwunderDialog()
        {
            InitializeComponent();

            cb_bundesland.ItemsSource = Enum.GetValues(typeof(Bundesland));
            cb_bundesland.SelectedIndex = 0;
        }

        private void lb_images_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lb_images.SelectedItem != null)
            {
                int index = lb_images.SelectedIndex;
                images.RemoveAt(index);
                safeimages.RemoveAt(index);
                lb_images.Items.RemoveAt(index);
            }
        }

        private void buttonOpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            openFileDialog.Title = "Bilder auswählen";

            if (openFileDialog.ShowDialog() == true)
            {
                images.Add(openFileDialog.FileName);
                safeimages.Add(openFileDialog.SafeFileName);
            }
            lb_images.Items.Add(openFileDialog.SafeFileName);


        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_id.Text) || String.IsNullOrEmpty(tb_name.Text) || String.IsNullOrEmpty(tb_beschreibung.Text) || String.IsNullOrEmpty(tb_longitude.Text) || String.IsNullOrEmpty(tb_latitude.Text) || String.IsNullOrEmpty(tb_art.Text))
            {
                MessageBox.Show("Bitte die Daten vollständig ausfüllen!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                success = false;
                return;
            }

            Id = int.Parse(tb_id.Text);
            Name = tb_name.Text;
            Beschreibung = tb_beschreibung.Text;
            Longitude = decimal.Parse(tb_longitude.Text);
            Latitude = decimal.Parse(tb_latitude.Text);
            Bundesland = (Bundesland)cb_bundesland.SelectedItem;
            Art = tb_art.Text;
            

            // if images exists sonst erstellen
            string imageDirPath = "./images/";

            if (!Directory.Exists(imageDirPath))
            {
                Directory.CreateDirectory(imageDirPath);
            }


            foreach (string image in images)
            {
                string fileName = Path.GetFileNameWithoutExtension(image);
                string extension = Path.GetExtension(image);

                string targetPath = Path.Combine(imageDirPath, fileName + extension);

                int counter = 1;

                string safeimagenew = "";

                while (File.Exists(targetPath))
                {

                    safeimagenew = $"{fileName}_{counter}{extension}";

                    targetPath = Path.Combine(
                        imageDirPath,
                        $"{fileName}_{counter}{extension}"
                    );

                    counter++;
                }

                File.Copy(image, targetPath);

                int indeximage = images.IndexOf(image);
                safeimages[indeximage] = safeimagenew;
            }

            // falls daten umbenannt werden
            Images = safeimages;


            success = true;
            Close();
        }
    }
}
