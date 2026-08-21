using Painter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Painter
{
    internal class Block : Expression
    {
        Program program = new Program();
        int blockStartLine;
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count == 0 || tokens[0].Type != Token.TokenType.OPEN_BRACE)
            {
                int currentLine = tokens.Count > 0 ? tokens[0].LineNumber : 1;
                Errors.Add($"Zeile {currentLine}: Expected '{{' at the beginning of a block.");
                return;
            }
            blockStartLine = tokens[0].LineNumber;
            tokens.RemoveAt(0);

            program.Parse(tokens);
            

            if (tokens.Count == 0 || tokens[0].Type != Token.TokenType.CLOSE_BRACE)
            {
                Errors.Add($"Zeile {blockStartLine}: Expected '}}' at the end of a block");
                return;
            }
            tokens.RemoveAt(0);
        }

        internal override void Execute(PainterControl painterControl)
        {
            program.Execute(painterControl);
        }
    }
}
