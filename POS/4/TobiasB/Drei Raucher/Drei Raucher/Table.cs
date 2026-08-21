using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drei_Raucher
{
    internal class Table
    {
        public Zutaten ZutatEins { get => one; set { one = value;} }
        public Zutaten ZutatZwei { get => two; set { two = value; } }

        private Zutaten one;
        private Zutaten two;

        public bool tableFull()
        {
            if (one != Zutaten.None && two != Zutaten.None)
            {
                return true;
            }
            return false;
        }
    }
}
