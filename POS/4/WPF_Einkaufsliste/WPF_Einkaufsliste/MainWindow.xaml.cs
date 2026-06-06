using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using LINQtoCSV;

namespace WPF_Einkaufsliste
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ShoppingListItem> EinkaufsListe { get; set; } = new ObservableCollection<ShoppingListItem>();
        private List<Product> _allProducts = new List<Product>();

        public MainWindow()
        {
            InitializeComponent();

            // Setup der direkten Listen-Bindung
            EinkaufsListeBox.ItemsSource = EinkaufsListe;

            // Setup der TreeView Gruppierung per LINQ/WPF-CollectionView
            ICollectionView groupedView = CollectionViewSource.GetDefaultView(EinkaufsListe);
            groupedView.GroupDescriptions.Add(new PropertyGroupDescription("Kategorie"));
            EinkaufsListeTree.ItemsSource = groupedView.Groups;

            LoadCSV();
        }

        private void LoadCSV()
        {
            try
            {
                CsvContext cc = new CsvContext();
                CsvFileDescription inputFileDescription = new CsvFileDescription
                {
                    SeparatorChar = ';',
                    FirstLineHasColumnNames = false,
                    EnforceCsvColumnAttribute = true
                };

                // Use application base directory to resolve the path
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Produkte.csv");

                // LINQ: Produkte einlesen
                _allProducts = cc.Read<Product>(filePath, inputFileDescription).ToList();

                // LINQ: Kategorien extrahieren und sortieren
                var categories = _allProducts.Select(p => p.Kategorie).Distinct().OrderBy(k => k).ToList();
                CategoryComboBox.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der CSV: " + ex.Message);
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is string selectedCategory)
            {
                // LINQ: Passende Produkte zur Kategorie suchen
                ProductComboBox.ItemsSource = _allProducts
                    .Where(p => p.Kategorie == selectedCategory)
                    .OrderBy(p => p.Name)
                    .ToList();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = SearchTextBox.Text.ToLower().Trim();
            if (!string.IsNullOrEmpty(search))
            {
                // LINQ: Suche über alle Produkte
                var results = _allProducts
                    .Where(p => p.Name.ToLower().Contains(search) || p.Kategorie.ToLower().Contains(search))
                    .OrderBy(p => p.Name)
                    .ToList();

                SearchComboBox.ItemsSource = results;
                if (results.Any()) SearchComboBox.SelectedIndex = 0; // Direkt das erste selektieren
            }
            else
            {
                SearchComboBox.ItemsSource = null;
            }
        }

        private void Hinzufuegen_Click(object sender, RoutedEventArgs e)
        {
            string targetName = null;
            string targetCategory = "Eigene / Diverses";
            int menge = (int)MengeSlider.Value;

            // Priorität der Auswahl nach Vorgabe
            if (!string.IsNullOrWhiteSpace(CustomProductTextBox.Text))
            {
                // 1. Individuelle Eingabe
                targetName = CustomProductTextBox.Text.Trim();
            }
            else if (SearchComboBox.SelectedItem is Product searchedProd)
            {
                // 2. Such-Ergebnis
                targetName = searchedProd.Name;
                targetCategory = searchedProd.Kategorie;
            }
            else if (ProductComboBox.SelectedItem is Product catalogProd)
            {
                // 3. Katalog Auswahl
                targetName = catalogProd.Name;
                targetCategory = catalogProd.Kategorie;
            }

            if (string.IsNullOrEmpty(targetName)) return;

            // LINQ: Prüfen, ob Artikel bereits in Liste vorhanden ist
            var existingItem = EinkaufsListe.FirstOrDefault(item =>
                item.Name.Equals(targetName, StringComparison.OrdinalIgnoreCase) &&
                item.Kategorie == targetCategory);

            if (existingItem != null)
            {
                existingItem.Menge += menge;
            }
            else
            {
                EinkaufsListe.Add(new ShoppingListItem
                {
                    Name = targetName,
                    Kategorie = targetCategory,
                    Menge = menge
                });
            }

            // Reset einiger Eingaben für den nächsten Workflow
            CustomProductTextBox.Clear();
            SearchTextBox.Clear();
            MengeSlider.Value = 1;
        }


        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EinkaufsListe.Clear();
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = EinkaufsListeBox != null && EinkaufsListeBox.SelectedItems.Count > 0;
        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // LINQ: Lösche alle ausgewählten Elemente
            var itemsToRemove = EinkaufsListeBox.SelectedItems.Cast<ShoppingListItem>().ToList();
            foreach (var item in itemsToRemove)
            {
                EinkaufsListe.Remove(item);
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var saveDialog = new Microsoft.Win32.SaveFileDialog { Filter = "XML Dateien (*.xml)|*.xml", DefaultExt = "xml" };
            if (saveDialog.ShowDialog() == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<ShoppingListItem>));
                using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                {
                    serializer.Serialize(writer, EinkaufsListe);
                }
            }
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var openDialog = new Microsoft.Win32.OpenFileDialog { Filter = "XML Dateien (*.xml)|*.xml" };
            if (openDialog.ShowDialog() == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<ShoppingListItem>));
                using (StreamReader reader = new StreamReader(openDialog.FileName))
                {
                    var loadedList = (ObservableCollection<ShoppingListItem>)serializer.Deserialize(reader);
                    EinkaufsListe.Clear();
                    foreach (var item in loadedList)
                    {
                        EinkaufsListe.Add(item);
                    }
                }
            }
        }

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Druckt eine graphische Repräsentation der Listen-Ansicht
                printDialog.PrintVisual(EinkaufsListeBox, "Einkaufsliste");
            }
        }
    }
}