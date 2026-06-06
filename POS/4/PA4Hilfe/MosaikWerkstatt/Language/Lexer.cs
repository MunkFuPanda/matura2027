using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MosaikWerkstatt.Language
{
    // ---------------------------------------------------------------------
    // TOKENIZER / LEXER  (Teilaufgabe "Regular Expressions -> Tokens")
    //
    // Wandelt den rohen Quelltext in eine Liste von Tokens um.
    // Verwendet EINE Master-Regex mit benannten Gruppen. Das ist genau
    // der Stil, den die Painter-Angabe verlangt.
    //
    // Anpassen bei neuer Sprache:
    //  - Brauchst du andere Symbole (z.B. Klammern (), Komma, "+"),
    //    fuegst du oben eine weitere benannte Gruppe hinzu.
    //  - WORD/NUMBER bleiben fast immer gleich.
    // ---------------------------------------------------------------------
    public class Lexer
    {
        // Reihenfolge zaehlt: die erste passende Gruppe gewinnt.
        // \G + fortlaufender Index = "matche genau ab dieser Position"
        private static readonly Regex TokenRegex = new Regex(
            @"\G(" +
            @"(?<WS>\s+)" +                 // Whitespace (wird verworfen)
            @"|(?<NUMBER>\d+)" +            // Zahl
            @"|(?<WORD>[A-Za-z][A-Za-z\-]*)" + // Wort (Buchstaben + '-', z.B. IS-A, CAN)
            @"|(?<LBRACE>\{)" +
            @"|(?<RBRACE>\})" +
            @")",
            RegexOptions.Compiled);

        public List<Token> Tokenize(string source)
        {
            var tokens = new List<Token>();
            int pos = 0;
            int line = 1;
            int lineStart = 0; // Zeichen-Index, an dem die aktuelle Zeile beginnt

            while (pos < source.Length)
            {
                Match m = TokenRegex.Match(source, pos);
                if (!m.Success || m.Index != pos)
                {
                    // Kein bekanntes Token -> ungueltiges Zeichen melden (mit Zeile!)
                    int col = pos - lineStart + 1;
                    throw new ParseException(
                        "Ungueltiges Zeichen '" + source[pos] + "'", line, col);
                }

                int column = pos - lineStart + 1;
                string text = m.Value;

                if (m.Groups["WS"].Success)
                {
                    // Zeilenumbrueche zaehlen, damit Zeilennummern stimmen.
                    // WICHTIG: ueber den Index k laufen, damit lineStart auf den
                    // ABSOLUTEN Index hinter dem '\n' zeigt (auch bei mehreren
                    // Umbruechen oder Whitespace vor dem '\n').
                    for (int k = 0; k < text.Length; k++)
                    {
                        if (text[k] == '\n')
                        {
                            line++;
                            lineStart = pos + k + 1; // hinter dem '\n'
                        }
                    }
                }
                else if (m.Groups["NUMBER"].Success)
                    tokens.Add(new Token(TokenType.Number, text, line, column));
                else if (m.Groups["WORD"].Success)
                    tokens.Add(new Token(TokenType.Word, text, line, column));
                else if (m.Groups["LBRACE"].Success)
                    tokens.Add(new Token(TokenType.LBrace, text, line, column));
                else if (m.Groups["RBRACE"].Success)
                    tokens.Add(new Token(TokenType.RBrace, text, line, column));

                pos += m.Length;
                // lineStart muss bei WS schon korrekt gesetzt sein (siehe oben)
            }

            // Endmarker: erleichtert dem Parser das "ist es zu Ende?"-Pruefen.
            tokens.Add(new Token(TokenType.End, "<EOF>", line, 1));
            return tokens;
        }
    }
}
