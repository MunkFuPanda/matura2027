using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Painter.Models;

namespace Painter.Lexer
{
    /// <summary>
    /// Der Tokenizer wandelt Quellcode in eine Liste von Tokens um.
    /// Dies ist der erste Schritt des Compiler-Prozesses (Lexikalische Analyse).
    /// 
    /// Wir verwenden Regular Expressions, um die verschiedenen Token-Typen zu erkennen.
    /// </summary>
    public class Tokenizer
    {
        // Regular Expression Muster für die verschiedenen Token-Typen
        private static class TokenPatterns
        {
            // Whitespace (Leerzeichen und Tabs) - wird ignoriert
            public static readonly string WHITESPACE = @"^[ \t]+";

            // Kommentare (optional, nicht in der Basis-Aufgabe)
            // public static readonly string COMMENT = @"^#.*";

            // Keywords und Identifizierer (case-insensitive)
            public static readonly string KEYWORD = @"^(TURN|LEFT|RIGHT|DRAW|COLOR|FOR|Red|Green|Blue|Yellow|White|Black|Cyan|Magenta|Gray)(?=[\s\{\}]|$)";

            // Zahlen (positive Ganzzahlen)
            public static readonly string NUMBER = @"^\d+";

            // Geschweifte Klammern
            public static readonly string LBRACE = @"^\{";
            public static readonly string RBRACE = @"^\}";

            // Zeilenumbruch
            public static readonly string NEWLINE = @"^[\r\n]+";
        }

        // Definiert die Farbnamen, die gültig sind
        private static readonly HashSet<string> VALID_COLORS = new HashSet<string>
        {
            "RED", "GREEN", "BLUE", "YELLOW", "WHITE", "BLACK", "CYAN", "MAGENTA", "GRAY"
        };

        // Definiert die Keywords
        private static readonly HashSet<string> KEYWORDS = new HashSet<string>
        {
            "TURN", "LEFT", "RIGHT", "DRAW", "COLOR", "FOR"
        };

        /// <summary>
        /// Tokenisiert den Quellcode in eine Liste von Tokens.
        /// </summary>
        /// <param name="source">Der zu tokenisierende Quellcode</param>
        /// <returns>Liste von Tokens</returns>
        public List<Token> Tokenize(string source)
        {
            var tokens = new List<Token>();

            if (string.IsNullOrEmpty(source))
            {
                tokens.Add(new Token(TokenType.EOF, "", 1, 1));
                return tokens;
            }

            int lineNumber = 1;
            int columnNumber = 1;
            int index = 0;

            while (index < source.Length)
            {
                // Versuche Whitespace zu erkennen (wird übersprungen)
                Match match = Regex.Match(source.Substring(index), TokenPatterns.WHITESPACE);
                if (match.Success)
                {
                    columnNumber += match.Length;
                    index += match.Length;
                    continue;
                }

                // Versuche Zeilenumbruch zu erkennen
                match = Regex.Match(source.Substring(index), TokenPatterns.NEWLINE);
                if (match.Success)
                {
                    tokens.Add(new Token(TokenType.NEWLINE, match.Value, lineNumber, columnNumber));
                    // Zähle alle Zeilenumbrüche in diesem Match
                    string newlineContent = match.Value;
                    foreach (char c in newlineContent)
                    {
                        if (c == '\n' || c == '\r')
                        {
                            if (c == '\n')
                                lineNumber++;
                        }
                    }
                    columnNumber = 1;
                    index += match.Length;
                    continue;
                }

                // Versuche Keyword (oder Farbname) zu erkennen
                match = Regex.Match(source.Substring(index), TokenPatterns.KEYWORD);
                if (match.Success)
                {
                    string keyword = match.Value.ToUpper();
                    var token = CreateKeywordToken(keyword, lineNumber, columnNumber);
                    tokens.Add(token);
                    columnNumber += match.Length;
                    index += match.Length;
                    continue;
                }

                // Versuche Zahl zu erkennen
                match = Regex.Match(source.Substring(index), TokenPatterns.NUMBER);
                if (match.Success)
                {
                    tokens.Add(new Token(TokenType.NUMBER, match.Value, lineNumber, columnNumber));
                    columnNumber += match.Length;
                    index += match.Length;
                    continue;
                }

                // Versuche öffnende geschweifte Klammer zu erkennen
                match = Regex.Match(source.Substring(index), TokenPatterns.LBRACE);
                if (match.Success)
                {
                    tokens.Add(new Token(TokenType.LBRACE, match.Value, lineNumber, columnNumber));
                    columnNumber += match.Length;
                    index += match.Length;
                    continue;
                }

                // Versuche schließende geschweifte Klammer zu erkennen
                match = Regex.Match(source.Substring(index), TokenPatterns.RBRACE);
                if (match.Success)
                {
                    tokens.Add(new Token(TokenType.RBRACE, match.Value, lineNumber, columnNumber));
                    columnNumber += match.Length;
                    index += match.Length;
                    continue;
                }

                // Wenn nichts passt: Unbekannter Token (Fehler)
                char unknownChar = source[index];
                tokens.Add(new Token(TokenType.UNKNOWN, unknownChar.ToString(), lineNumber, columnNumber));
                columnNumber++;
                index++;
            }

            // Füge EOF-Token hinzu
            tokens.Add(new Token(TokenType.EOF, "", lineNumber, columnNumber));

            return tokens;
        }

        /// <summary>
        /// Erstellt den richtigen Token für ein erkanntes Keyword.
        /// </summary>
        private Token CreateKeywordToken(string keyword, int lineNumber, int columnNumber)
        {
            if (KEYWORDS.Contains(keyword))
            {
                return new Token((TokenType)Enum.Parse(typeof(TokenType), keyword), keyword, lineNumber, columnNumber);
            }

            if (VALID_COLORS.Contains(keyword))
            {
                return new Token(TokenType.COLOR_NAME, keyword, lineNumber, columnNumber);
            }

            // Sollte nicht vorkommen, da das Regex nur gültige Keywords/Farben akzeptiert
            return new Token(TokenType.UNKNOWN, keyword, lineNumber, columnNumber);
        }
    }
}
