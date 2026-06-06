
using AbcRobotCore;

namespace Roboter_4C_2026
{
    internal class RepeatExpression : Expression
    {
        int count = 0;
        Block block = new Block();
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.NUMBER)
            {
                count = int.Parse(tokens[0].Value);
                tokens.RemoveAt(0); // Entferne die Zahl
            }
            else
            {
                Errors.Add($"Expected a number, got" + (tokens.Count > 0 ? tokens[0].Value : "end of input"));
            }
            block.Parse(tokens);
        }

        internal override void Execute(RobotField roboter)
        {
            for (int i = 0; i < count; i++)
            {
                block.Execute(roboter);
            }
        }
    }
}