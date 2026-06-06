namespace WPF_Painter
{
    internal class ColorExpression : Expression
    {
        private string colorName = "Black";

        internal override void Parse(List<Token> tokens)
        {
            // Erwartet: [LETTER] (z.B. "Red")
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.LETTER)
            {
                colorName = tokens[0].Value;
                tokens.RemoveAt(0);
            }
            else
            {
                Errors.Add("COLOR erwartet einen Farbnamen.");
            }
        }

        internal override void Interpret(PainterContext context)
        {
            context.Painter.ChangeColor(colorName);
        }
    }
}