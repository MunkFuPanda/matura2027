using DataModels;
using LinqToDB;
using Painter;
using System.Diagnostics;

namespace UEBUNG_FORMEN
{
    internal class FormExpression : Expression
    {

        public string Name { get; set; }
        internal override void Parse(List<Token> tokens)
        {
            using var db = new FormenDB(
                new DataOptions().UseSQLite($"Data Source={System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db", "Formen.db")}")
            );

            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.WORD)
            {
                var formen = new List<Forman>();
                formen = db.Formen.Where(w => w.Name == tokens[0].Value).ToList();
                Debug.WriteLine($"Gefundene Formen mit Name '{tokens[0].Value}': {formen.Count}");

                if (formen.Count == 1)
                {
                    Name = tokens[0].Value;
                    tokens.RemoveAt(0); // Entferne die X-Koordinate
                }
                else
                {
                    Errors.Add($"Zeile {tokens[0].LineNumber}: Ungültiges Wort: {tokens[0].Value}");
                    tokens.RemoveAt(0); // Entferne das ungültige Token, um die Analyse fortzusetzen
                }
            }
            else
            {
                Errors.Add("Erwartet: Word nach FORM");
            }
        }

        internal override void Run(PainterControl painter)
        {
            var lines = new List<Line>();
            using var db = new FormenDB(
                new DataOptions().UseSQLite($"Data Source={System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db", "Formen.db")}")
            );
            lines = db.Lines.Where(w => w.Forman.Name == Name).ToList();

            int x = 0;
            int y = 0;

            if (lines.Count > 0)
            {
                // Gehe unsichtbar zum ersten Startpunkt der Form, anstatt eine Linie dorthin zu ziehen
                var firstLine = lines[0];
                painter.Move((int)(firstLine.X ?? 0), (int)(firstLine.Y ?? 0));

                // Zeichne die eigentlichen Linien für die restlichen Punkte
                for (int i = 1; i < lines.Count; i++)
                {
                    var line = lines[i];
                    Debug.WriteLine($"Zeichne Linie zu: X={line.X}, Y={line.Y}");
                    painter.Line((int)(line.X ?? 0), (int)(line.Y ?? 0));
                    x += (int)(line.X ?? 0);
                    y += (int)(line.Y ?? 0);
                }
                painter.Line((int)(-x), (int)(-y));
            }
        }
    }
}