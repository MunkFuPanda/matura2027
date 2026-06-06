using System;
using System.Collections.Generic;
using System.Windows.Media;
using Painter.Models;
using Painter.Interpreter.Commands;
using static Painter.Interpreter.Commands.TurnCommand;

namespace Painter.Interpreter
{
    /// <summary>
    /// Die ParseException wird geworfen, wenn der Parser einen Fehler bei der Analyse des Codes findet.
    /// Sie enthält die Zeilennummer, Spaltennummer und eine aussagekräftige Fehlermeldung.
    /// </summary>
    public class ParseException : Exception
    {
        public int LineNumber { get; }
        public int ColumnNumber { get; }

        public ParseException(string message, int lineNumber = 0, int columnNumber = 0)
            : base(message)
        {
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        public override string ToString()
        {
            if (LineNumber > 0)
                return $"Fehler in Zeile {LineNumber}, Spalte {ColumnNumber}: {Message}";
            return $"Fehler: {Message}";
        }
    }

    /// <summary>
    /// Der Parser (der zweite Schritt nach der Tokenisierung) konvertiert die Token-Liste
    /// in eine Struktur von Command-Objekten.
    /// 
    /// Der Parser verwendet rekursives Descent Parsing (auch known als Predictive Parsing):
    /// - Die grammatikalischen Regeln werden direkt als Parsing-Methoden implementiert
    /// - Jede Methode behandelt eine grammatikalische Regel
    /// - Sie rufen sich gegenseitig auf, um komplexere Strukturen zu analysieren
    /// </summary>
    public class Parser
    {
        private List<Token> tokens;
        private int currentTokenIndex;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.currentTokenIndex = 0;
        }

        /// <summary>
        /// Hauptmethode zum Parsen eines kompletten Programms.
        /// Ein Programm besteht aus mehreren Statements.
        /// </summary>
        public ICommand[] ParseProgram()
        {
            var commands = new List<ICommand>();

            // Überspringe Newlines am Anfang
            SkipNewlines();

            // Parse Statements bis zum End-of-File
            while (!IsAtEnd())
            {
                // Überspringe weitere Newlines zwischen Statements
                SkipNewlines();

                if (IsAtEnd())
                    break;

                commands.Add(ParseStatement());

                // Überspringe Newlines nach einem Statement
                SkipNewlines();
            }

            return commands.ToArray();
        }

        /// <summary>
        /// Parst ein einzelnes Statement.
        /// Ein Statement kann sein: TURN, DRAW, COLOR, FOR-Schleife oder Block
        /// </summary>
        private ICommand ParseStatement()
        {
            if (Check(TokenType.TURN))
                return ParseTurnCommand();

            if (Check(TokenType.DRAW))
                return ParseDrawCommand();

            if (Check(TokenType.COLOR))
                return ParseColorCommand();

            if (Check(TokenType.FOR))
                return ParseForLoop();

            if (Check(TokenType.LBRACE))
                return ParseBlock();

            // Fehler: Unbekanntes Statement
            throw new ParseException(
                $"Unerwartetes Token: '{CurrentToken().Value}'. Erwartet: TURN, DRAW, COLOR, FOR oder {{",
                CurrentToken().LineNumber,
                CurrentToken().ColumnNumber);
        }

        /// <summary>
        /// Parst einen TURN-Befehl.
        /// Format: TURN LEFT <zahl> oder TURN RIGHT <zahl>
        /// </summary>
        private ICommand ParseTurnCommand()
        {
            Token turnToken = Consume(TokenType.TURN, "Erwartete TURN");
            int lineNumber = turnToken.LineNumber;
            int columnNumber = turnToken.ColumnNumber;

            // Prüfe auf LEFT oder RIGHT
            TurnDirection direction;
            if (Check(TokenType.LEFT))
            {
                Advance();
                direction = TurnDirection.LEFT;
            }
            else if (Check(TokenType.RIGHT))
            {
                Advance();
                direction = TurnDirection.RIGHT;
            }
            else
            {
                throw new ParseException(
                    "Nach TURN muss LEFT oder RIGHT folgen",
                    CurrentToken().LineNumber,
                    CurrentToken().ColumnNumber);
            }

            // Parst die Winkelzahl
            Token numberToken = ParseNumberToken();
            int angle;

            if (!int.TryParse(numberToken.Value, out angle))
            {
                throw new ParseException(
                    $"'{numberToken.Value}' ist keine gültige Zahl",
                    numberToken.LineNumber,
                    numberToken.ColumnNumber);
            }

            return new TurnCommand(direction, angle);
        }

        /// <summary>
        /// Parst einen DRAW-Befehl.
        /// Format: DRAW <zahl>
        /// </summary>
        private ICommand ParseDrawCommand()
        {
            Token drawToken = Consume(TokenType.DRAW, "Erwartete DRAW");

            Token numberToken = ParseNumberToken();
            int length;

            if (!int.TryParse(numberToken.Value, out length))
            {
                throw new ParseException(
                    $"'{numberToken.Value}' ist keine gültige Zahl",
                    numberToken.LineNumber,
                    numberToken.ColumnNumber);
            }

            if (length <= 0)
            {
                throw new ParseException(
                    $"Die Linienlände muss größer als 0 sein, erhalten: {length}",
                    numberToken.LineNumber,
                    numberToken.ColumnNumber);
            }

            return new DrawCommand(length);
        }

        /// <summary>
        /// Parst einen COLOR-Befehl.
        /// Format: COLOR <farbenname>
        /// </summary>
        private ICommand ParseColorCommand()
        {
            Token colorToken = Consume(TokenType.COLOR, "Erwartete COLOR");
            int lineNumber = colorToken.LineNumber;
            int columnNumber = colorToken.ColumnNumber;

            // Prüfe ob ein Farbname folgt
            if (!Check(TokenType.COLOR_NAME))
            {
                throw new ParseException(
                    "Nach COLOR muss eine Farbe folgen (Red, Green, Blue, Yellow, White, Black, Cyan, Magenta, Gray)",
                    CurrentToken().LineNumber,
                    CurrentToken().ColumnNumber);
            }

            Token colorNameToken = Advance();
            Color color = ParseColor(colorNameToken.Value);

            return new ColorCommand(color);
        }

        /// <summary>
        /// Parst eine FOR-Schleife.
        /// Format: FOR <zahl> { ... statements ... }
        /// </summary>
        private ICommand ParseForLoop()
        {
            Token forToken = Consume(TokenType.FOR, "Erwartete FOR");
            int lineNumber = forToken.LineNumber;

            // Parse die Anzahl der Wiederholungen
            Token numberToken = ParseNumberToken();
            int count;

            if (!int.TryParse(numberToken.Value, out count))
            {
                throw new ParseException(
                    $"'{numberToken.Value}' ist keine gültige Zahl",
                    numberToken.LineNumber,
                    numberToken.ColumnNumber);
            }

            // Prüfe auf öffnende Klammer
            if (!Check(TokenType.LBRACE))
            {
                throw new ParseException(
                    "Nach FOR <zahl> muss ein Block {{ ... }} folgen",
                    CurrentToken().LineNumber,
                    CurrentToken().ColumnNumber);
            }

            // Parse den Block
            ICommand block = ParseBlock();

            return new RepeatCommand(count, new[] { block });
        }

        /// <summary>
        /// Parst einen Block von Statements.
        /// Format: { ... statements ... }
        /// </summary>
        private ICommand ParseBlock()
        {
            Consume(TokenType.LBRACE, "Erwartete {");
            var commands = new List<ICommand>();

            SkipNewlines();

            // Parse Statements bis zur schließenden Klammer
            while (!Check(TokenType.RBRACE) && !IsAtEnd())
            {
                if (Check(TokenType.NEWLINE))
                {
                    Advance();
                    continue;
                }

                commands.Add(ParseStatement());
                SkipNewlines();
            }

            if (!Check(TokenType.RBRACE))
            {
                throw new ParseException(
                    "Schließende Klammer } erwartet",
                    CurrentToken().LineNumber,
                    CurrentToken().ColumnNumber);
            }

            Consume(TokenType.RBRACE, "Erwartete }");

            return new BlockCommand(commands.ToArray());
        }

        /// <summary>
        /// Hilfsmethode zum Parsen eines NUMBER-Tokens.
        /// Wirft eine ParseException, falls kein NUMBER-Token folgt.
        /// </summary>
        private Token ParseNumberToken()
        {
            if (!Check(TokenType.NUMBER))
            {
                throw new ParseException(
                    "Eine Zahl erwartet",
                    CurrentToken().LineNumber,
                    CurrentToken().ColumnNumber);
            }

            return Advance();
        }

        /// <summary>
        /// Konvertiert einen Farbnamen-String in ein Color-Objekt.
        /// </summary>
        private Color ParseColor(string colorName)
        {
            return colorName.ToUpper() switch
            {
                "RED" => Colors.Red,
                "GREEN" => Colors.Green,
                "BLUE" => Colors.Blue,
                "YELLOW" => Colors.Yellow,
                "WHITE" => Colors.White,
                "BLACK" => Colors.Black,
                "CYAN" => Colors.Cyan,
                "MAGENTA" => Colors.Magenta,
                "GRAY" => Colors.Gray,
                _ => throw new ParseException($"Unbekannte Farbe: {colorName}")
            };
        }

        // ========================= Hilfsmethoden für das Parsing =========================

        /// <summary>
        /// Gibt das aktuelle Token zurück, ohne es zu konsumieren.
        /// </summary>
        private Token CurrentToken()
        {
            if (currentTokenIndex >= tokens.Count)
                return tokens[tokens.Count - 1]; // Rückgabe des EOF-Tokens
            return tokens[currentTokenIndex];
        }

        /// <summary>
        /// Gibt das aktuelle Token zurück und bewegt sich zum nächsten.
        /// </summary>
        private Token Advance()
        {
            if (!IsAtEnd())
                currentTokenIndex++;
            return tokens[currentTokenIndex - 1];
        }

        /// <summary>
        /// Prüft, ob das aktuelle Token vom erwarteten Typ ist.
        /// </summary>
        private bool Check(TokenType type)
        {
            if (IsAtEnd())
                return false;
            return CurrentToken().Type == type;
        }

        /// <summary>
        /// Wenn das aktuelle Token vom erwarteten Typ ist, konsumiert es es.
        /// Ansonsten wirft eine ParseException.
        /// </summary>
        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
                return Advance();

            throw new ParseException(
                $"{message}. Erhalten: {CurrentToken().Value}",
                CurrentToken().LineNumber,
                CurrentToken().ColumnNumber);
        }

        /// <summary>
        /// Prüft, ob wir am Ende der Token-Liste sind.
        /// </summary>
        private bool IsAtEnd()
        {
            return currentTokenIndex >= tokens.Count - 1 || CurrentToken().Type == TokenType.EOF;
        }

        /// <summary>
        /// Überspringt alle NEWLINE-Tokens.
        /// </summary>
        private void SkipNewlines()
        {
            while (Check(TokenType.NEWLINE) && !IsAtEnd())
            {
                Advance();
            }
        }
    }
}
