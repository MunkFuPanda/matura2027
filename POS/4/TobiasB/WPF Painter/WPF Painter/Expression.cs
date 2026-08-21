using Painter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Painter
{
    internal abstract class Expression
    {
        internal static List<String> Errors = new List<string>();

        internal abstract void Parse(List<Token> tokens);
        
        internal virtual void Execute(PainterControl painterControl)
        {

        }

    }
}
