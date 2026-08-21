using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Einkauflistengenerator
{
    public class Product : INotifyPropertyChanged
    {
        [CsvColumn(FieldIndex = 1)]
        public string Category { get; set; }
        [CsvColumn(FieldIndex = 2)]
        public string Name { get; set; }

        private int quantity;

        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
