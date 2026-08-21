using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku
{
    public class PlayingField
    {
        public Field[,] board;
        public ObservableCollection<Field> fields = new();
        public int size;

        public PlayingField(int size)
        {
            this.size = size;
            board = new Field[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = new Field('N', j, i);
                    fields.Add(new Field('N', j, i));
                }
            }
        }
    }
}
