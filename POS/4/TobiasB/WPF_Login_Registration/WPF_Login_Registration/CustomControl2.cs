using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPF_Login_Registration
{
    public class Registration : Control
    {
        static Registration()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Registration), new FrameworkPropertyMetadata(typeof(Registration)));
        }
    }
}
