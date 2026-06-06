using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter_4C_2026
{
    internal class Condition : Expression
    {
        private Token direction = new Token();
        private Token target = new Token();

        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count >= 3 && 
                (tokens[0].Value == "UP" || tokens[0].Value == "DOWN" || tokens[0].Value == "LEFT" || tokens[0].Value == "RIGHT") &&
                tokens[1].Value == "IS-A")
            {
                direction = tokens[0];
                target = tokens[2];
                tokens.RemoveRange(0, 3); // Remove DIRECTION, IS-A, TARGET
            }
            else
            {
                Errors.Add("Invalid condition format. Expected '[DIRECTION] IS-A OBSTACLE' or '[DIRECTION] IS-A [LETTER]'.");
            }
        }

        internal bool Evaluate(AbcRobotCore.RobotField roboter)
        {
            var dir = AbcRobotCore.RobotField.Direction.Down;
            if (direction != null)
            {
                switch (direction.Value)
                {
                    case "UP": dir = AbcRobotCore.RobotField.Direction.Up; break;
                    case "DOWN": dir = AbcRobotCore.RobotField.Direction.Down; break;
                    case "LEFT": dir = AbcRobotCore.RobotField.Direction.Left; break;
                    case "RIGHT": dir = AbcRobotCore.RobotField.Direction.Right; break;
                }
            }
            if (target != null && target.Value == "OBSTACLE")
                return roboter.IsObstacle(dir);
            else if (target != null)
                return roboter.IsLetter(target.Value, dir);
            else
                return false;
        }
    }
}
