using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter_4C_2026
{
    internal class Block : Expression
    {
        Program program = new Program();
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count == 0 || tokens[0].Type != Token.TokenType.OPEN_BRACE)
            {
                Errors.Add("Expected '{' at the beginning of a block.");
                return;
            }
            tokens.RemoveAt(0); // Entferne die öffnende Klammer '{'

            program.Parse(tokens);

            if (tokens.Count == 0 || tokens[0].Type != Token.TokenType.CLOSE_BRACE)
            {
                Errors.Add("Expected '}' at the end of a block.");
                return;
            }
            tokens.RemoveAt(0); // Entferne die schließende Klammer '}'
        }

        internal override void Execute(RobotField roboter)
        {
            program.Execute(roboter);
        }
    }
}
