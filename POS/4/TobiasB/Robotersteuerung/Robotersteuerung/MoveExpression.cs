
using AbcRobotCore;

namespace Robotersteuerung
{
    internal class MoveExpression : Expression
    {
        Token direction = new Token() { Type = Token.TokenType.KEYWORD, Value = ""};
        Token direction2 = new Token() { Type = Token.TokenType.KEYWORD, Value = "" };

        internal override void Parse(List<Token> tokens)
        {
            // Direction 1 (Required)
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.KEYWORD && (tokens[0].Value == "UP" | tokens[0].Value == "DOWN" | tokens[0].Value == "LEFT" || tokens[0].Value == "RIGHT"))
            {
                direction = tokens[0];
                tokens.RemoveAt(0);
            }
            else
            {
                // Token nicht da 0 anzahl oder den token value
                Errors.Add($"Expected Direction keyword ");
                tokens.RemoveAt(0);
            }

            // Direction 2 (Optional)
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.KEYWORD && (tokens[0].Value == "UP" | tokens[0].Value == "DOWN" | tokens[0].Value == "LEFT" || tokens[0].Value == "RIGHT"))
            {
                direction2 = tokens[0];
                tokens.RemoveAt(0);
            }
        }

        internal override void Execute(RobotField roboter)
        {

            bool success = true;

            switch (direction.Value)
            {
                case "UP":
                    if (direction2.Value != "")
                    {
                        // Second direction einbauen
                        success = roboter.Move(RobotField.Direction.Up, getDir(direction2.Value));
                    }
                    else
                    {
                       success = roboter.Move(RobotField.Direction.Up);
                    }
                       

                    break;
                case "DOWN":
                    if (direction2.Value != "")
                    {
                        // Second direction einbauen
                        success = roboter.Move(RobotField.Direction.Down, getDir(direction2.Value));
                    }
                    else
                    {
                        success = roboter.Move(RobotField.Direction.Down);
                    }
                    break;
                case "LEFT":
                    if (direction2.Value != "")
                    {
                        // Second direction einbauen
                        success = roboter.Move(RobotField.Direction.Left, getDir(direction2.Value));
                    }
                    else
                    {
                        success = roboter.Move(RobotField.Direction.Left);
                    }
                    break;
                case "RIGHT":
                    if (direction2.Value != "")
                    {
                        // Second direction einbauen
                        success = roboter.Move(RobotField.Direction.Right, getDir(direction2.Value));
                    }
                    else
                    {
                        success = roboter.Move(RobotField.Direction.Right);
                    }
                    break;
                default:
                    Errors.Add("RUNNING: Direction not set");
                    // sollte nicht passieren
                    break;

            }
            if (!success)
            {
                Errors.Add("RUNNING: Could not move in Direction");
            }

        }

        public RobotField.Direction getDir(String direction)
        {
            switch (direction)
            {
                case "UP":
                    return RobotField.Direction.Up;
                    break;
                case "DOWN":
                    return RobotField.Direction.Down;
                    break;
                case "RIGHT":
                    return RobotField.Direction.Right;
                    break;
                case "LEFT":
                    return RobotField.Direction.Left;
                    break;
                default:
                    return 0;
                    break;

            }
                
        }

        
    }

    
}