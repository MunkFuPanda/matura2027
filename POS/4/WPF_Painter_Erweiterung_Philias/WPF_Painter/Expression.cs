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
        internal virtual void Execute(Painter.PainterControl roboter) 
        {
            // Defualt implementation does nothing, can be overridden by subclasses if needed
        }
    }
}
