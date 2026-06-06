namespace Painter.Models
{
    /// <summary>
    /// Repräsentiert einen Token aus dem Quellcode.
    /// Ein Token ist die kleinste bedeutungsvolle Einheit der Programmiersprache.
    /// </summary>
    public class Token
    {
        // Typ des Tokens (z.B. TURN, DRAW, NUMBER, etc.)
        public TokenType Type { get; set; }

        // Der tatsächliche Text des Tokens aus dem Quellcode
        public string Value { get; set; }

        // Zeilennummer, auf der der Token erscheint (für Fehlerbehandlung)
        public int LineNumber { get; set; }

        // Spaltennummer, auf der der Token beginnt (für Fehlerbehandlung)
        public int ColumnNumber { get; set; }

        public Token(TokenType type, string value, int lineNumber, int columnNumber)
        {
            Type = type;
            Value = value;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        public override string ToString()
        {
            return $"Token({Type}, '{Value}', Line {LineNumber}, Col {ColumnNumber})";
        }
    }

    /// <summary>
    /// Definiert alle möglichen Token-Typen in der Programmiersprache
    /// </summary>
    public enum TokenType
    {
        // Keywords
        TURN,
        LEFT,
        RIGHT,
        DRAW,
        COLOR,
        FOR,

        // Literale
        NUMBER,      // Ganzzahl (z.B. 45, 250, 6)
        COLOR_NAME,  // Farbnamen (z.B. Red, Blue, Green)

        // Symbole
        LBRACE,      // {
        RBRACE,      // }

        // Spezielle Token
        NEWLINE,     // Zeilenumbruch
        EOF,         // Ende der Datei
        UNKNOWN      // Unbekannter Token (Fehler)
    }
}
