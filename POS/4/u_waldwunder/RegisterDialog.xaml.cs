using DataModels;
using LinqToDB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Net.Mime.MediaTypeNames;

namespace u_waldwunder
{
    /// <summary>
    /// Interaction logic for RegisterDialog.xaml
    /// </summary>
    public partial class RegisterDialog : Window
    {
        ObservableCollection<BildItem> bilder = new ObservableCollection<BildItem>();
        int counter = 0;
        public RegisterDialog()
        {
            InitializeComponent();
            ImageListBox.ItemsSource = bilder;
        }

        private void addImageButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                BildItem bild = new BildItem
                {
                    Path = openFileDialog.FileName,
                    Name = System.IO.Path.GetFileName(openFileDialog.FileName),
                    Id = counter++
                };
                bilder.Add(bild);
            }
        }

        private void removePicture(object sender, MouseButtonEventArgs e)
        {
            bilder.Remove((BildItem)ImageListBox.SelectedItem);
        }

        private void cancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void registerButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text) || string.IsNullOrWhiteSpace(ProvinzTextBox.Text) || string.IsNullOrWhiteSpace(TypeTextBox.Text))
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(LatitudeTextBox.Text, out double latitude) || !double.TryParse(LongitudeTextBox.Text, out double longitude))
            {
                MessageBox.Show("Bitte geben Sie gültige Koordinaten ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (latitude < 46.308597 || latitude > 49.063175 || longitude < 9.362383 || longitude > 17.231941)
            {
                MessageBox.Show("Die Koordinaten müssen innerhalb von Österreich liegen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!double.TryParse(RatingTextBox.Text, out double rating))
            {
                MessageBox.Show("Bitte geben Sie einen gültigen Wert für das Wunder ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (bilder.Count == 0)
            {
                MessageBox.Show("Bitte fügen Sie mindestens ein Bild hinzu.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using var db = new WaldwunderDB(
                    new DataOptions().UseSQLite($"Data Source={System.IO.Path.Combine(@"C:\Users\Philias\source\repos\u_waldwunder\", "db", "Waldwunder.db")}")
                );

                var waldwunder = new Waldwunder
                {
                    Name = NameTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    Province = ProvinzTextBox.Text,
                    Type = TypeTextBox.Text,
                    Latitude = (decimal?)double.Parse(LatitudeTextBox.Text),
                    Longitude = (decimal?)double.Parse(LongitudeTextBox.Text),
                    Votes = (decimal?)double.Parse(RatingTextBox.Text),
                };

                db.Insert(waldwunder);

                var id = db.GetTable<Waldwunder>().Where(w => w.Name == waldwunder.Name && w.Description == waldwunder.Description && w.Province == waldwunder.Province && w.Type == waldwunder.Type && w.Latitude == waldwunder.Latitude && w.Longitude == waldwunder.Longitude && w.Votes == waldwunder.Votes).Select(w => w.Id).FirstOrDefault();

                foreach (var bild in bilder)
                {
                    string newname = CopyWithUniqueName(bild.Path, System.IO.Path.Combine(@"C:\Users\Philias\source\repos\u_waldwunder\ressourcen\", "images"));

                    var newBild = new Bilder
                    {
                        Name = newname,
                        Wonder = id
                    };
                    db.Insert(newBild);
                }
                MessageBox.Show("Das Wunder wird registriert. Bitte warten Sie einen Moment.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern des Wunders: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }

            Close();
        }

        public static string CopyWithUniqueName(string sourceFile, string targetFolder)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(sourceFile);
            string extension = System.IO.Path.GetExtension(sourceFile);

            string targetFile = System.IO.Path.Combine(targetFolder, fileName + extension);

            int counter = 2;

            while (System.IO.File.Exists(targetFile))
            {
                targetFile = System.IO.Path.Combine(
                    targetFolder,
                    $"{fileName}_{counter}{extension}");

                counter++;
            }

            System.IO.File.Copy(sourceFile, targetFile);

            return targetFile;
        }
    }
}
