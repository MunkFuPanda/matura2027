using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Gomoku
{
    public class Item : INotifyPropertyChanged
    {
        public int X { get; set; }
        public int Y { get; set; }

        private string character = "";

        public string Charater
        {
            get => character;
            set
            {
                character = value;
                OnPropertyChanged(nameof(Charater));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public Item(int x, int y, String character)
        {
            this.X = x;
            this.Y = y;
            this.character = character;
        }

        public Item()
        {

        }
    }
}
