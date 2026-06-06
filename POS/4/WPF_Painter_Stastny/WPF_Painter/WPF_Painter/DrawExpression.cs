namespace WPF_Painter
{
    internal class DrawExpression : Expression
    {
        private int distance = 0;

        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.NUMBER)
            {
                distance = int.Parse(tokens[0].Value);
                tokens.RemoveAt(0);
            }
            else
            {
                Errors.Add("DRAW erwartet eine Zahl.");
            }
        }

        internal override void Interpret(PainterContext context)
        {
            context.Painter.Draw(distance);
        }
    }
}