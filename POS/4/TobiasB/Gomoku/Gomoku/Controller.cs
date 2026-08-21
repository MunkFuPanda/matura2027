using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku
{
    public abstract class Controller
    {
        public abstract int Input(PlayingField playingField, int x, int y, char player);
    }
}
