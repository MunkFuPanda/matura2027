using System.Diagnostics;
using DataModels;
using LinqToDB;
using Painter;
using PainterPA;

namespace PainterPA {
    internal class FormExpression : Expression {

        public string Name { get; set; }
        internal override void Parse(List<Token> tokenList) {
            using var db = new FormenDB(
                new DataOptions().UseSQLite("Data Source=Formen.db")
            );

            if (tokenList.Count > 0 && tokenList[0].Type == Token.TokenType.WORD) {
                var formen = new List<Forman>();
                formen = db.Formen.Where(w => w.Name == tokenList[0].Value).ToList();
                Debug.WriteLine($"Gefundene Formen mit Name '{tokenList[0].Value}': {formen.Count}");

                if (formen.Count == 1) {
                    Name = tokenList[0].Value;
                    tokenList.RemoveAt(0); // Entferne die X-Koordinate
                } else {
                    Errors.Add($"Zeile {tokenList[0].LineNumber}: Ungültiges Wort: {tokenList[0].Value}");
                    tokenList.RemoveAt(0); // Entferne das ungültige Token, um die Analyse fortzusetzen
                }
            } else {
                Errors.Add("Erwartet: Word nach FORM");
            }
        }

        internal override void Run(PainterControl painter) {
            var lines = new List<Line>();
            using var db = new FormenDB(
                new DataOptions().UseSQLite("Data Source=Formen.db")
            );
            lines = db.Lines.Where(w => w.Forman.Name == Name).ToList();

            int x = 0;
            int y = 0;

            if (lines.Count > 0) {
                // Gehe unsichtbar zum ersten Startpunkt der Form, anstatt eine Linie dorthin zu ziehen
                var firstLine = lines[0];
                painter.Move((int)(firstLine.X ?? 0), (int)(firstLine.Y ?? 0));

                // Zeichne die eigentlichen Linien für die restlichen Punkte
                for (int i = 1; i < lines.Count; i++) {
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