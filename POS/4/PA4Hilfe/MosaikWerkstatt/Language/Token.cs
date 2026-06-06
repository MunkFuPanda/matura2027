using System.Collections.Generic;

namespace MosaikWerkstatt.Language
{
    // ---------------------------------------------------------------------
    // Die Art eines Tokens. Bewusst MINIMAL gehalten:
    // Der Lexer klassifiziert nur grob (Wort / Zahl / Klammer).
    // OB ein Wort ein Schluesselwort, eine Richtung oder eine Farbe ist,
    // entscheidet erst der Parser ueber den Text. Das haelt den Lexer
    // sprachunabhaengig -> bei einer neuen Angabe musst du ihn kaum anfassen.
    // ---------------------------------------------------------------------
    public enum TokenType
    {
        Word,    // Buchstaben: SET, MOVE, RIGHT, Red, WHILE, CAN ...
        Number,  // Ziffern: 3, 42
        LBrace,  // {
        RBrace,  // }
        End      // kuenstliches Endmarker-Token (EOF)
    }

    // Ein einzelnes Token mit Position fuer die Fehlermeldungen.
    public class Token
    {
        public TokenType Type { get; }
        public string Text { get; }
        public int Line { get; }   // 1-basiert, fuer "Fehler in Zeile X"
        public int Column { get; } // 1-basiert

        public Token(TokenType type, string text, int line, int column)
        {
            Type = type;
            Text = text;
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return Type + "('" + Text + "') @" + Line + ":" + Column;
        }
    }
}
