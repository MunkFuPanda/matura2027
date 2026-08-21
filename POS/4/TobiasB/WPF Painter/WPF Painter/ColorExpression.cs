
using Painter;

namespace WPF_Painter
{
    internal class ColorExpression : Expression
    {
        int linenumber;

        Token color = new Token() { Type = Token.TokenType.LETTERS, Value = "" };

        internal ColorExpression(int linenumber)
        {
            this.linenumber = linenumber;
        }
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.LETTERS)
            {
                color = tokens[0];
                tokens.RemoveAt(0);
            }
            else
            {
                Errors.Add($"Zeile {this.linenumber}: Expected colortype ");
                tokens.RemoveAt(0);
            }
        }

        internal override void Execute(PainterControl painterControl)
        {
            painterControl.ChangeColor(color.Value);
        }
    }
}