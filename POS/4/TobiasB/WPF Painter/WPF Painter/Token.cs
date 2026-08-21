using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Painter
{
    public class Token
    {
        public enum TokenType
        {
            NUMBER,
            LETTERS,
            OPEN_BRACE,
            CLOSE_BRACE,
            KEYWORD,
            ERROR,
            NEWLINE
        }

        public TokenType Type { get; set; }

        public string Value { get; set; }

        public int LineNumber { get; set; }
    }
}
