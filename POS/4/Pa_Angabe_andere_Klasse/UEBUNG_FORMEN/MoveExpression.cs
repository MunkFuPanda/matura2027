using Painter;

namespace UEBUNG_FORMEN
{
    internal class MoveExpression : Expression
    {
        public int X { get; set; }
        public int Y { get; set; }
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 1 && tokens[0].Type == Token.TokenType.NUMBER && tokens[1].Type == Token.TokenType.NUMBER)
            {
                if (int.TryParse(tokens[0].Value, out int x))
                {
                    X = x;
                    tokens.RemoveAt(0); // Entferne die X-Koordinate
                }
                else
                {
                    Errors.Add($"Zeile {tokens[0].LineNumber}: Ungültige Zahl: {tokens[0].Value}");
                    tokens.RemoveAt(0); // Entferne das ungültige Token, um die Analyse fortzusetzen
                }

                if (int.TryParse(tokens[0].Value, out int y))
                {
                    Y = y;
                    tokens.RemoveAt(0); // Entferne die Y-Koordinate
                }
                else
                {
                    Errors.Add($"Zeile {tokens[0].LineNumber}: Ungültige Zahl: {tokens[0].Value}");
                    tokens.RemoveAt(0); // Entferne das ungültige Token, um die Analyse fortzusetzen
                }
            }
            else
            {
                Errors.Add("Erwartet: Zahl nach MOVE");
            }
        }

        internal override void Run(PainterControl painter)
        {
            painter.Move(X, Y);
        }
    }
}