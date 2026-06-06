namespace WPF_Painter
{
    internal class TurnExpression : Expression
    {
        private string direction = "RIGHT";
        private int angle = 0;

        internal override void Parse(List<Token> tokens)
        {
            // Erwartet: [KEYWORD: LEFT/RIGHT] [NUMBER]
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.KEYWORD)
            {
                direction = tokens[0].Value.ToUpper();
                tokens.RemoveAt(0);
            }
            else
            {
                Errors.Add("TURN erwartet eine Richtung (LEFT/RIGHT).");
            }

            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.NUMBER)
            {
                angle = int.Parse(tokens[0].Value);
                tokens.RemoveAt(0);
            }
            else
            {
                Errors.Add("TURN erwartet eine Zahl für den Winkel.");
            }
        }

        internal override void Interpret(PainterContext context)
        {
            if (direction == "LEFT")
                context.Painter.Rotate(-angle); // Negativ für Linksdrehung
            else
                context.Painter.Rotate(angle);  // Positiv für Rechtsdrehung
        }
    }
}