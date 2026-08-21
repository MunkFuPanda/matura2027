
using Painter;

namespace WPF_Painter
{
    internal class DrawExpression : Expression
    {
        int distance;

        int linenumber;

        internal DrawExpression(int linenumber)
        {
            this.linenumber = linenumber;
        }
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.NUMBER)
            {
                distance = int.Parse(tokens[0].Value);
                tokens.RemoveAt(0);
            }
        }

        internal override void Execute(PainterControl painterControl)
        {
            painterControl.Draw(distance);
        }
    }
}