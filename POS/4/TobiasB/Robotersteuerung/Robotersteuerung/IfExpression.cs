using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using static AbcRobotCore.RobotField;

namespace Robotersteuerung
{
    internal class IfExpression : Expression
    {
        Condition condition = new Condition();
        Block block = new Block();

        internal override void Parse(List<Token> tokens)
        {
            // man kann auch nur beiden parsen und passt

            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.KEYWORD)
            {
                condition.Parse(tokens);
                
            }
            else
            {
                // Token nicht da 0 anzahl oder den token value
                Errors.Add($"Expected Condition, got " + (tokens.Count > 0 ? tokens[0].Value : "end of input"));
                tokens.RemoveAt(0);
            }
            block.Parse(tokens);
        }

        internal override void Execute(RobotField roboter)
        {
            if (condition.Evaluate(roboter))
            {
                block.Execute(roboter);
            } 
        }
    }
}
