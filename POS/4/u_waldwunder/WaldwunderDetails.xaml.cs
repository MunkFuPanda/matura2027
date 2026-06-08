using DataModels;
using System;
using System.Collections.Generic;
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

namespace u_waldwunder
{
    /// <summary>
    /// Interaction logic for WaldwunderDetails.xaml
    /// </summary>
    public partial class WaldwunderDetails : Window
    {
        public Waldwunder selectedWaldwunder;
        public List<Bilder> allBilder = new List<Bilder>();

        public WaldwunderDetails()
        {
            InitializeComponent();
        }

        internal void SetWaldwunder(Waldwunder selectedWaldwunder, List<Bilder> bilder)
        {
            this.selectedWaldwunder = selectedWaldwunder;
            this.allBilder = bilder;

            NameTextBlock.Text = selectedWaldwunder.Name;
            DescriptionTextBlock.Text = selectedWaldwunder.Description;
            ProvinzTextBlock.Text = selectedWaldwunder.Province;
            TypeTextBlock.Text = selectedWaldwunder.Type;

            for (int i = 0; i < allBilder.Count; i++) { 
                allBilder[i].Name = System.IO.Path.Combine("C:\\Users\\Philias\\source\\repos\\u_waldwunder\\ressourcen\\images\\", allBilder[i].Name);
            }
            ImageListBox.ItemsSource = allBilder.Select(b => b.Name);
        }
    }
}
