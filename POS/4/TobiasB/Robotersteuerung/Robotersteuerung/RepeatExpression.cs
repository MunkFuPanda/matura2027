
using AbcRobotCore;

namespace Robotersteuerung
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
                tokens.RemoveAt(0);
            }
            else
            {
                // Token nicht da 0 anzahl oder den token value
                Errors.Add($"Expected Number, got " + (tokens.Count > 0 ? tokens[0].Value : "end of input"));
                tokens.RemoveAt(0);
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