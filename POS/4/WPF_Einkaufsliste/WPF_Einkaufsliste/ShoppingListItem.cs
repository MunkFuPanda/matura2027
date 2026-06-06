using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_Einkaufsliste
{
    public class ShoppingListItem : INotifyPropertyChanged
    {
        private string _name;
        private string _kategorie;
        private int _menge;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Kategorie
        {
            get => _kategorie;
            set { _kategorie = value; OnPropertyChanged(); }
        }

        public int Menge
        {
            get => _menge;
            set { _menge = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}