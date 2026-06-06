using System;
using System.Collections.Generic;

namespace Roboter_4C_2026
{
    internal class UntilExpression : Expression
    {
        Condition condition = new Condition();
        Block block = new Block();

        internal override void Parse(List<Token> tokens)
        {
            condition.Parse(tokens);
            block.Parse(tokens);
        }

        internal override void Execute(AbcRobotCore.RobotField roboter)
        {
            // Execute the block UNTIL the condition becomes true
            while (!condition.Evaluate(roboter))
            {
                block.Execute(roboter);
            }
        }
    }
}
