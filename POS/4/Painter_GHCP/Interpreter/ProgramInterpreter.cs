using System;
using Painter.Interpreter.Commands;
using Painter.Lexer;
using Painter.Models;

namespace Painter.Interpreter
{
    /// <summary>
    /// Der Interpreter ist das Herzstück des Programms.
    /// Er verbindet alle Komponenten:
    /// 1. Tokenizer: Wandelt Code in Tokens um
    /// 2. Parser: Wandelt Tokens in Commands um
    /// 3. Executor: Führt die Commands aus
    /// </summary>
    public class ProgramInterpreter
    {
        private Tokenizer tokenizer;
        private Parser parser;

        public ProgramInterpreter()
        {
            tokenizer = new Tokenizer();
        }

        /// <summary>
        /// Interpretiert den Quellcode und gibt den zeichnerischen Kontext zurück.
        /// Dies ist der Haupteinstiegspunkt für die Ausführung eines Programms.
        /// </summary>
        /// <param name="sourceCode">Der zu interpretierende Quellcode</param>
        /// <returns>Der DrawingContext mit den gezeichneten Linien</returns>
        /// <exception cref="ParseException">Bei Syntaxfehlern im Code</exception>
        public DrawingContext Execute(string sourceCode)
        {
            // Schritt 1: Tokenisierung (Lexikalische Analyse)
            var tokens = tokenizer.Tokenize(sourceCode);

            // Schritt 2: Parsing (Syntaktische Analyse)
            parser = new Parser(tokens);
            ICommand[] commands;

            try
            {
                commands = parser.ParseProgram();
            }
            catch (ParseException ex)
            {
                // Die ParseException wird mit Zeilennummer und Spalte weitergeleitet
                throw;
            }

            // Schritt 3: Ausführung der Commands
            DrawingContext context = new DrawingContext();

            // Führe alle top-level Commands aus
            foreach (var command in commands)
            {
                command.Execute(context);
            }

            // Gib den gesammelten Kontext zurück
            return context;
        }
    }
}
