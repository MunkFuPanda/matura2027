using DataModels;
using LinqToDB;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WaldWunder
{
    public partial class MainWindow : Window
    {
        // Kommentare sind nicht alle von mir, die meisten schon, manche von KI, wie die Methoden namen! ~Stastny

        // Globale Listen für den Bilder-Upload im Dialog
        private List<string> ImagePath = new List<string>();
        public ObservableCollection<string> ImageNames { get; set; } = new ObservableCollection<string>();

        // Die zentrale Datenbank-Konfiguration für Linq2db
        private readonly DataOptions _dbOptions;

        public MainWindow()
        {
            InitializeComponent();

            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Model", "Waldwunder.db");
            _dbOptions = new DataOptions().UseSQLite($"Data Source={dbPath};");

            string targetDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            SelectedImagesListBox.ItemsSource = ImageNames;

            // Holt im endefekt die dAten aus der DB ohne etwas einzufügen,
            // keine ahnung wie man es besser lösen kann.
            this.Loaded += (s, e) =>
            {
                this.UpdateLayout();
                LoadAllData(); // <-- Holt die Daten beim Starten aus der DB in die ListBox
            };

            MapCanvas.SizeChanged += (s, e) =>
            {
                if (WaldwunderListBox.ItemsSource is List<Waldwunder> aktuelleListe)
                {
                    ShowWaldwunderOnMap(aktuelleListe);
                }
            };
        }

        // METHODE 1: Alle Daten laden und Benutzeroberfläche + Karte aktualisieren
        private void LoadAllData()
        {
            using (var db = new WaldwunderDB(_dbOptions))
            {
                // 1. Daten frisch aus der DB abfragen
                var daten = db.Waldwunders.ToList();

                // 2. Die ListBox zwingen, sich komplett neu zu zeichnen
                WaldwunderListBox.ItemsSource = null;  // Zuerst nullen, um alten Cache zu löschen
                WaldwunderListBox.ItemsSource = daten; // dann Neu zuweisen

                // 3. Pins auf der Karte zeichnen
                ShowWaldwunderOnMap(daten);
            }
        }

        // METHODE 2: Die mathematische Berechnung für die Pins auf der Grafik
        private void ShowWaldwunderOnMap(List<Waldwunder> wunderListe)
        {
            // 1. Alle alten vorschauen vom Canvas löschen
            MapCanvas.Children.Clear();

            // Die Eckpunkte aus der Österreich-Karte
            double mapMaxLat = 49.063175; // Oben
            double mapMinLat = 46.308597; // Unten
            double mapMinLon = 9.362383;  // Links
            double mapMaxLon = 17.231941; // Rechts

            // Aktuelle Pixel-Größe des Canvas auf den Bildschirm anpassen.
            double canvasWidth = MapCanvas.ActualWidth;
            double canvasHeight = MapCanvas.ActualHeight;

            // Falls das UI beim Start noch nicht bereit ist (Breite/Höhe 0), kurz abbrechen
            if (canvasWidth == 0 || canvasHeight == 0) return;

            // 1. Öffnung der DB, um zu schauen, welche Bild-Dateinamen den Wundern zugeordnet sind
            using (var db = new WaldwunderDB(_dbOptions))
            {
                foreach (var wunder in wunderListe)
                {
                    if (wunder.Latitude.HasValue && wunder.Longitude.HasValue)
                    {
                        // 2. Das erste zugehörige Bild aus der DB für dieses spezifische Waldwunder holen
                        var erstesBildEintrag = db.Bilders
                            .FirstOrDefault(b => b.Wonder == wunder.Id);

                        string imagePath = null;

                        if (erstesBildEintrag != null)
                        {
                            // Prüfung beide Ordner separat auf der Festplatte!
                            string pfadInNewImages = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "new_images", erstesBildEintrag.Name);
                            string pfadInTestdaten = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Testdaten", "images", erstesBildEintrag.Name);

                            // Wenn die Datei in new_images existiert, nimmt's diesen Pfad
                            if (File.Exists(pfadInNewImages))
                            {
                                imagePath = pfadInNewImages;
                            }
                            // Ansonsten schaut's nach, ob sie im verschachtelten Testdaten-Ordner liegt
                            else if (File.Exists(pfadInTestdaten))
                            {
                                imagePath = pfadInTestdaten;
                            }
                        }

                        // UI-Element für den Marker auf der Karte
                        UIElement kartenMarker;

                        // 3. Wenn das Bild in einem der beiden Ordner gefunden wurde -> baue eines Mini-Foto
                        if (imagePath != null)
                        {
                            var thumbnail = new Image
                            {
                                Source = new BitmapImage(new Uri(imagePath)),
                                Width = 40,   // Breite des Mini-Bildes
                                Height = 40,  // Höhe des Mini-Bildes
                                Stretch = System.Windows.Media.Stretch.UniformToFill // Schneidet das Foto sauber quadratisch zu
                            };

                            // Ein weißer Rahmen, damit man es auf dem Satellitenbild gut siehtW
                            kartenMarker = new Border
                            {
                                BorderBrush = System.Windows.Media.Brushes.White,
                                BorderThickness = new Thickness(2),
                                CornerRadius = new CornerRadius(5), // Leicht abgerundete Ecken
                                Child = thumbnail,
                                Width = 44,
                                Height = 44
                            };
                        }
                        else
                        {
                            // FALLBACK: Wenn das Waldwunder kein Bild hat (oder der Dateiname nirgends existiert),
                            // zeichnen ich hier einen auffälligen roten Punkt, damit man sieht, dass trotzdem die Koordinaten stimmen!
                            kartenMarker = new System.Windows.Shapes.Ellipse
                            {
                                Fill = System.Windows.Media.Brushes.Red,
                                Stroke = System.Windows.Media.Brushes.White,
                                StrokeThickness = 1.5,
                                Width = 14,
                                Height = 14
                            };
                        }

                        // 4. Dreisatz-Berechnung: GPS-Koordinaten in exakte Pixelposition umrechnen
                        // Hab ich so beim nachschauen im internet gefunden die Berechnung ~Stastny
                        double lat = (double)wunder.Latitude.Value;
                        double lon = (double)wunder.Longitude.Value;

                        double x = (lon - mapMinLon) / (mapMaxLon - mapMinLon) * canvasWidth;
                        double y = (mapMaxLat - lat) / (mapMaxLat - mapMinLat) * canvasHeight;

                        // 5. Den Marker zentriert auf die berechneten Pixel setzen
                        // Ein Bild (44x44) um 22 Pixel nach links/oben verschieben;
                        // den roten Punkt um 7 Pixel.
                        double offset = (imagePath != null) ? 22 : 7;
                        Canvas.SetLeft(kartenMarker, x - offset);
                        Canvas.SetTop(kartenMarker, y - offset);

                        // Ab auf das Canvas (die Karte)!
                        MapCanvas.Children.Add(kartenMarker);
                    }
                }
            }
        }

        // METHODE 3: Neues Waldwunder in der DB registrieren und Bilder kopieren
        private void Register_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new WaldwunderDB(_dbOptions))
                {
                    // 1. Neues Waldwunder-Objekt mit den Textbox-Daten erstellen
                    var neu = new Waldwunder
                    {
                        Name = InputName.Text,
                        Description = InputDescription.Text,
                        Latitude = decimal.Parse(InputLat.Text),
                        Longitude = decimal.Parse(InputLon.Text),
                    };

                    // 2. In der DB speichern und die frisch generierte ID (Primary Key) zurückerhalten
                    // Hab nicht dran gedacht, dass man in der DB auch ein autoincrement für PK's einstellen kann! ~Stastny
                    int newID = Convert.ToInt32(db.InsertWithInt32Identity(neu));

                    // Zielordner für die neuen Bilder ist "new_images"
                    string targetDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "new_images");

                    // Sicherstellen, dass der Ordner existiert (falls er im bin-Verzeichnis gelöscht wurde)
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    // 3. Alle im Dialog ausgewählten Bilder verarbeiten
                    foreach (string source in ImagePath)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(source);
                        string ext = Path.GetExtension(source);
                        string target = Path.Combine(targetDir, fileName + ext);

                        // Duplikate im Ordner verhindern (z.B. falls "Ahorn1.jpg" schon existiert -> "Ahorn1_1.jpg")
                        int i = 1;
                        while (File.Exists(target))
                        {
                            target = Path.Combine(targetDir, $"{fileName}_{i}{ext}");
                            i++;
                        }

                        // Bild physisch auf die Festplatte in "new_images" kopieren
                        File.Copy(source, target);

                        // 4. WICHTIG: Den Bild-Eintrag in die Datenbank-Tabelle schreiben!
                        // Ich speicher den reinen Dateinamen (z.B. "MeinBaum_1.jpg") und die ID des Waldwunders
                        var b = new Bilder
                        {
                            Name = Path.GetFileName(target), // Holt nur den Dateinamen ohne den ganzen C:\... Pfad
                            Wonder = newID                   // Die Verknüpfung (Foreign Key) zum gerade erstellten Wunder
                        };

                        db.Insert(b);
                    }
                }

                // die Benutzeroberfläche neu laden (lädt alle Daten frisch aus der DB und zeichnet die Bilder auf der Karte)
                LoadAllData();

                // Dialog schließen und Felder leeren
                HideDialog_Click(null, null);
                MessageBox.Show("Erfolgreich registriert und Bilder gespeichert!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern: " + ex.Message);
            }
        }

        // METHODE 4: Stichwortsuche (Name oder Beschreibung)
        private void SearchKeyword_Click(object sender, RoutedEventArgs e)
        {
            string search = txtSearch.Text.ToLower();
            using (var db = new WaldwunderDB(_dbOptions))
            {
                var gefiltert = db.Waldwunders
                    .Where(w => w.Name.ToLower().Contains(search) || w.Description.ToLower().Contains(search))
                    .ToList();

                WaldwunderListBox.ItemsSource = gefiltert;
                ShowWaldwunderOnMap(gefiltert); // Nur die Suchtreffer auf der Karte anzeigen

            }
        }

        // METHODE 5: Umkreissuche anhand von Koordinaten
        private void SearchLocation_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(InputLat.Text, out decimal lat) || !decimal.TryParse(InputLon.Text, out decimal lon)) return;

            using (var db = new WaldwunderDB(_dbOptions))
            {
                var gefiltert = db.Waldwunders
                    .Where(w => w.Latitude >= lat - 0.5m && w.Latitude <= lat + 0.5m &&
                                w.Longitude >= lon - 0.5m && w.Longitude <= lon + 0.5m)
                    .ToList();

                WaldwunderListBox.ItemsSource = gefiltert;
                ShowWaldwunderOnMap(gefiltert);
            }
        }

        // METHODE 6: Details anzeigen (durchsucht beide Bild-Ordner)
        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            var selected = WaldwunderListBox.SelectedItem as Waldwunder;

            if (selected == null)
            {
                MessageBox.Show("Bitte wähle zuerst ein Waldwunder aus!");
                return;
            }

            using (var db = new WaldwunderDB(_dbOptions))
            {
                var belongingImages = db.Bilders
                    .Where(b => b.Wonder == selected.Id)
                    .ToList();

                List<string> detailZeilen = new List<string>();

                foreach (var img in belongingImages)
                {
                    string neuerPfad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", img.Name);
                    string testPfad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Testdaten", img.Name);

                    if (File.Exists(neuerPfad))
                    {
                        detailZeilen.Add($"- {img.Name} (Ordner: images)");
                    }
                    else if (File.Exists(testPfad))
                    {
                        detailZeilen.Add($"- {img.Name} (Ordner: Testdaten)");
                    }
                    else
                    {
                        detailZeilen.Add($"- {img.Name} (Datei fehlt auf der Festplatte!)");
                    }
                }

                string alleBilderText = detailZeilen.Count > 0
                    ? string.Join("\n", detailZeilen)
                    : "Keine Bilder für dieses Waldwunder hinterlegt.";

                MessageBox.Show(
                    $"Titel: {selected.Name}\n" +
                    $"Beschreibung: {selected.Description}\n" +
                    $"Ort (Lat/Lon): {selected.Latitude} / {selected.Longitude}\n\n" +
                    $"Bilder:\n{alleBilderText}",
                    "Details zum Waldwunder"
                );
            }
        }

        //HILFSMETHODEN FÜR BENUTZEROBERFLÄCHE

        private void AddButton_Click(object sender, RoutedEventArgs e) => DialogOverlay.Visibility = Visibility.Visible;

        private void HideDialog_Click(Object sender, RoutedEventArgs e)
        {
            DialogOverlay.Visibility = Visibility.Collapsed;
            ImagePath.Clear();
            ImageNames.Clear();
            InputName.Clear();
            InputDescription.Clear();
            InputLat.Text = "48.0"; // Auf Standardwerte zurückgesetzt
            InputLon.Text = "16.0";
        }

        private void SelectImages_Click(Object sender, RoutedEventArgs e)
        {
            string testdatenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Testdaten");

            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Bilder|*.jpg;*.png",
                InitialDirectory = Directory.Exists(testdatenPath) ? testdatenPath : AppDomain.CurrentDomain.BaseDirectory
            };

            if (ofd.ShowDialog() == true)
            {
                foreach (string path in ofd.FileNames)
                {
                    ImagePath.Add(path);
                    ImageNames.Add(Path.GetFileName(path));
                }
            }
        }
    }
}