using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Gomoku.Model
{
    public enum CellState
    {
        Empty,
        Black,
        White
    }

    
    public class BoardModel
    {
        public int size = 9;
        public ObservableCollection<Item> Cells { get; set; } = new ObservableCollection<Item>();

        public BoardModel(int size)
        {
            this.size = size;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Cells.Add(new Item ( i, j, ""));
                }
            }
        }
    }
}
