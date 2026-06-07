using System;
using System.Collections.Generic;
using System.Text;

namespace PainterPA {
    internal class Token {
        internal enum TokenType {
            KEYWORD,
            CLOSE_BRACE,
            OPEN_BRACE,
            WORD,
            NUMBER,
            ERROR
        }

        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int LineNumber { get; set; }
    }
}