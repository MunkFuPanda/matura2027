using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Gomoku.Models {
    internal class Board : UniformGrid {
        private Field[] fields = new Field[15 * 15];
        public Board() {
            for (int i = 0; i < fields.Length; i++) {
                fields[i] = new Field();
                this.Children.Add(fields[i]);
            }
        }
    }
}
