
using Painter;

namespace WPF_Painter
{
    internal class TurnExpression : Expression
    {
        // -1 left +1 right
        int dir = 0;
        int angle;

        int linenumber;

        internal TurnExpression(int linenumber)
        {
            this.linenumber = linenumber;
        }
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.KEYWORD)
            {
                if (tokens[0].Value == "RIGHT")
                {
                    dir = 1;
                }
                else if (tokens[0].Value == "LEFT")
                {
                    dir = -1;
                }
                tokens.RemoveAt(0);
            }
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.NUMBER)
            {
                angle = int.Parse(tokens[0].Value);
                tokens.RemoveAt(0);
            }

            else
            {
                // Token nicht da 0 anzahl oder den token value
                Errors.Add($"Zeile {this.linenumber}: Expected Number, got " + (tokens.Count > 0 ? tokens[0].Value : "end of input"));
                tokens.RemoveAt(0);
            }
        }

        internal override void Execute(PainterControl painterControl)
        {
            // error handling missing
            if (dir == -1)
            {
                painterControl.Rotate(-angle);
            }
            else if (dir == 1)
            {
                painterControl.Rotate(angle);
            }
            else
            {
                // error handling
                return;
            }

                

        }
    }
}