using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gomoku.Models {
    internal class Field : Label {
        public Field() {
            this.Content = "";
            this.BorderThickness = new System.Windows.Thickness(1);
            this.BorderBrush = System.Windows.Media.Brushes.Blue;
            this.MouseDoubleClick += this.On_Click;
        }

        public void On_Click(object sender, MouseButtonEventArgs e) {
            this.Content = "X";
        }
    }
}
