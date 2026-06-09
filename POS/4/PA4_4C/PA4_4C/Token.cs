using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4_4C
{
    internal class Token
    {
        internal enum TokenType { Open, Close, Keyword, Identifier, Error }
        internal TokenType Type { get; set; }
        internal String Value { get; set; }
    }
}
