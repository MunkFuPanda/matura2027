using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotersteuerung
{
    public class Token
    {
        public enum TokenType
        {
            NUMBER,
            LETTER,
            CLOSE_BRACE,
            OPEN_BRACE,
            KEYWORD,
            ERROR
        }

        public TokenType Type { get; set; }
        public string Value { get; set; }
    }
}
