using LINQtoCSV;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace WPF_Einkauflistengenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        IEnumerable<Product> products;

        ObservableCollection<Product> einkaufsliste = new ObservableCollection<Product>();

        public MainWindow()
        {
            InitializeComponent();


            lb_einkaufsliste.ItemsSource = einkaufsliste;

            CsvContext cc = new CsvContext();

            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ';',
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true,
                FileCultureName = "de-AT"
            };

            // relativen nicht absoluten pfad verwenden noch schauen
            products =
                cc.Read<Product>("C:\\Users\\Tobias\\OneDrive - HTBLuVA Wiener Neustadt (1)\\Dokumente\\HTL\\4CHIF\\POS\\WPF Einkauflistengenerator\\WPF Einkauflistengenerator\\Produkte.csv", inputFileDescription);

            List<string> categories = products.Select(x => x.Category).Distinct().ToList();

            foreach (string cat in categories)
            {
                cb_cat.Items.Add(cat);
            }
            cb_cat.SelectedIndex = 0;

            List<string> firstcatname = products.Where(x => x.Category == cb_cat.SelectedItem.ToString()).Select(x => x.Name).ToList();

            foreach (string name in firstcatname)
            {
                cb_prod.Items.Add(name);
            }
        }

        private void cb_cat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_prod.Items.Clear();

            List<string> firstcatname = products.Where(x => x.Category == cb_cat.SelectedItem.ToString()).Select(x => x.Name).ToList();

            foreach (string name in firstcatname)
            {
                cb_prod.Items.Add(name);
            }

            cb_prod.SelectedIndex = 0;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_input.Text))
            {
                Product sel = products.Where(x => x.Category == cb_cat.SelectedItem.ToString() && x.Name == cb_prod.SelectedItem.ToString()).FirstOrDefault();

                var exists = einkaufsliste.Any(x => x.Category == sel.Category && x.Name == sel.Name);

                if (exists)
                {
                    Product prod = einkaufsliste.FirstOrDefault(x => x.Category == sel.Category && x.Name == sel.Name);

                    if (prod != null)
                    {
                        prod.Quantity += (int)sl_count.Value;
                    }
                }
                else
                {
                    sel.Quantity = (int)sl_count.Value;
                    einkaufsliste.Add(sel);
                }
            }
            else
            {
                Product sel = new Product();
                sel.Category = "Eigenes";
                sel.Name = tb_input.Text;
                sel.Quantity = (int)sl_count.Value;

                var exists = einkaufsliste.Any(x => x.Category == sel.Category && x.Name == sel.Name);

                if (exists)
                {
                    Product prod = einkaufsliste.FirstOrDefault(x => x.Category == sel.Category && x.Name == sel.Name);

                    if (prod != null)
                    {
                        prod.Quantity += (int)sl_count.Value;
                    }
                }

                else
                {
                    einkaufsliste.Add(sel);
                }
                
            }
        }
        #region CommandBindings
        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        #endregion


    }
}