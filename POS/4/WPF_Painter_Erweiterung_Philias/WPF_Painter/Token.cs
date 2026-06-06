using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Painter
{
    internal class Token
    {
        internal enum TokenType
        {
            KEYWORD,
            OPEN_BRACE,
            CLOSE_BRACE,
            WORD,
            NUMBER,
            ERROR
        }

        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int LineNumber { get; set; }
    }
}
