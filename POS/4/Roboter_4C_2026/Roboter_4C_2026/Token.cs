using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter_4C_2026
{
    internal class Token
    {
        internal enum TokenType
        {
            KEYWORD,
            OPEN_BRACE,
            CLOSE_BRACE,
            LETTER,
            NUMBER,
            ERROR
        }

        public TokenType Type { get; set; }
        public string Value { get; set; }
    }
}
