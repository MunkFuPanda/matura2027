
using Painter;
using System.Windows.Documents;

namespace WPF_Painter
{
    internal class ForExpression : Expression
    {
        int linenumber;
        int count = 0;
        Block block = new Block();

        internal ForExpression(int linenumber)
        {
            this.linenumber = linenumber;
        }
        internal override void Parse(List<Token> tokens)
        {
            // Skip newlines
            while (tokens.Count > 0 && tokens[0].Type == Token.TokenType.NEWLINE)
            {
                tokens.RemoveAt(0);
            }
            
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.NUMBER)
            {
                count = int.Parse(tokens[0].Value);
                tokens.RemoveAt(0);
            }
            else
            {
                // Token nicht da 0 anzahl oder den token value
                int errorLine = tokens.Count > 0 ? tokens[0].LineNumber : this.linenumber;
                Errors.Add($"Zeile {errorLine}: Expected Number, got " + (tokens.Count > 0 ? tokens[0].Value : "end of input"));
                if (tokens.Count > 0)
                    tokens.RemoveAt(0);
            }
            block.Parse(tokens);
        }

        internal override void Execute(PainterControl painterControl)
        {
            for (int i = 0; i < count; i++)
            {
                block.Execute(painterControl);
            }
        }
    }
}