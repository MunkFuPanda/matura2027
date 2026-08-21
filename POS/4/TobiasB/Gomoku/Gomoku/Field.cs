using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku
{
    public partial class Field : ObservableObject
    {
        [ObservableProperty]
        public char colour;

        public int x { get; set; }
        public int y { get; set; }

        public Field(char colour, int x, int y)
        {
            this.colour = colour;
            this.x = x;
            this.y = y;
        }
    }
}
