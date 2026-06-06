
using AbcRobotCore;

namespace Roboter_4C_2026
{
    internal class MoveExpression : Expression
    {
        List<Token> directions = new List<Token>();

        internal override void Parse(List<Token> tokens)
        {
            while (tokens.Count > 0 && tokens[0].Type == Token.TokenType.KEYWORD && 
                   (tokens[0].Value == "UP" || tokens[0].Value == "DOWN" || tokens[0].Value == "LEFT" || tokens[0].Value == "RIGHT"))
            {
                directions.Add(tokens[0]);
                tokens.RemoveAt(0);
            }

            if (directions.Count == 0)
            {
                Errors.Add($"Expected a direction keyword (UP, DOWN, LEFT, RIGHT), got " + (tokens.Count > 0 ? tokens[0].Value : "end of input"));
            }
        }

        internal override void Execute(RobotField roboter)
        {
            foreach (var dir in directions)
            {
                bool success = false;
                switch (dir.Value)
                {
                    case "UP":
                        success = roboter.Move(RobotField.Direction.Up);
                        break;
                    case "DOWN":
                        success = roboter.Move(RobotField.Direction.Down);
                        break;
                    case "LEFT":
                        success = roboter.Move(RobotField.Direction.Left);
                        break;
                    case "RIGHT":
                        success = roboter.Move(RobotField.Direction.Right);
                        break;
                    default:
                        Errors.Add("Direction not set");
                        break;
                }

                if (!success)
                {
                    Errors.Add($"Failed to move {dir.Value}. Possible obstacle or out of bounds.");
                }
            }
        }
    }
}