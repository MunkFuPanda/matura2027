namespace WPF_Painter
{
    internal class TurnExpression : Expression
    {
        public int Angle { get; set; }
        public String Direction { get; set; }
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 1 && tokens[1].Type == Token.TokenType.NUMBER && tokens[0].Type == Token.TokenType.KEYWORD)
            {
                if (int.TryParse(tokens[1].Value, out int angle))
                {
                    Angle = angle;
                    tokens.RemoveAt(1); // Entferne die Winkelangabe
                    Direction = tokens[0].Value.ToUpper(); // Speichere die Richtung (z.B. "RIGHT" oder "LEFT")
                    tokens.RemoveAt(0); // Entferne die Richtungsangabe
                }
                else
                {
                    Errors.Add($"Zeile {tokens[0].LineNumber}: Ungültige Zahl order/und Direction: {tokens[0].Value + tokens[1].Value}");
                    tokens.RemoveAt(0); // Entferne die ungültige Richtungsangabe, um die Analyse fortzusetzen
                    tokens.RemoveAt(0); // Entferne das ungültige Token, um die Analyse fortzusetzen
                }
            }
            else
            {
                Errors.Add("Erwartet: DIRECTION & Zahl nach TURN");
            }
        }
        internal override void Execute(Painter.PainterControl painter)
        {
            if (string.IsNullOrEmpty(Direction)) return;

            int angleToRotate = Angle;
            switch (Direction)
            {
                case "LEFT":
                    angleToRotate = -Angle;
                    break;
                case "RIGHT":
                    break;
                default:
                    Errors.Add($"Ungültige Richtung: {Direction}");
                    break;
            }
            painter.Rotate(angleToRotate);
        }
    }
}