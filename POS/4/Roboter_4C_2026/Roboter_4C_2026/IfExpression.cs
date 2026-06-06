using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter_4C_2026
{
    internal class IfExpression : Expression
    {
        Condition condition = new Condition();
        Block block = new Block();

        internal override void Parse(List<Token> tokens)
        {
            condition.Parse(tokens);
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