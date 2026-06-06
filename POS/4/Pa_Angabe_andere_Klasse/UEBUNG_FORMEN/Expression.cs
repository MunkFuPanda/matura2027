using Painter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEBUNG_FORMEN
{
    public abstract class Expression
    {
        internal static List<String> Errors { get; set; } = new List<String>();
        internal abstract void Parse(List<Token> tokenlist);
        internal abstract void Run(PainterControl painter);
    }
}