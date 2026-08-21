using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotersteuerung
{
    internal class Condition : Expression
    {
        Token direction = new Token() { Type = Token.TokenType.KEYWORD, Value = "" };
        Token obj = new Token() { Type = Token.TokenType.KEYWORD, Value = "" };


        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.KEYWORD && (tokens[0].Value == "UP" | tokens[0].Value == "DOWN" | tokens[0].Value == "LEFT" || tokens[0].Value == "RIGHT"))
            {
                direction = tokens[0];
                tokens.RemoveAt(0);
            }
            else
            {
                Errors.Add($"Expected Direction for Condition, got " + (tokens.Count > 0 ? tokens[0].Value : "end of input"));
                tokens.RemoveAt(0);
            }

            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.KEYWORD && tokens[0].Value == "IS-A")
            {
                tokens.RemoveAt(0);
            }
            else
            {
                Errors.Add($"Expected IS-A for Condition, got " + (tokens.Count > 0 ? tokens[0].Value : "end of input"));
                tokens.RemoveAt(0);
            }

            if (tokens.Count > 0 && (tokens[0].Type == Token.TokenType.KEYWORD || tokens[0].Type == Token.TokenType.LETTER))
            {
                obj = tokens[0];
                tokens.RemoveAt(0);
            }
            else
            {
                Errors.Add($"Expected LETTER for Condition, got " + (tokens.Count > 0 ? tokens[0].Value : "end of input"));
                tokens.RemoveAt(0);
            }
            


        }

        internal bool Evaluate(RobotField roboter)
        {
            bool result = false;

            if (obj.Value == "OBSTACLE")
            {

                switch (direction.Value)
                {
                    case "UP":
                        result = roboter.IsObstacle(RobotField.Direction.Up);
                        break;
                    case "DOWN":
                        result = roboter.IsObstacle(RobotField.Direction.Down);
                        break;
                    case "LEFT":
                        result = roboter.IsObstacle(RobotField.Direction.Left);
                        break;
                    case "RIGHT":
                        result = roboter.IsObstacle(RobotField.Direction.Right);
                        break;
                    default:
                        Errors.Add("RUNNING: Direction not set");
                        // sollte nicht passieren
                        break;

                }
            }

            else 
            {

                switch (direction.Value)
                {
                    case "UP":
                        result = roboter.IsLetter(obj.Value, RobotField.Direction.Up);
                        break;
                    case "DOWN":
                        result = roboter.IsLetter(obj.Value, RobotField.Direction.Down);
                        break;
                    case "LEFT":
                        result = roboter.IsLetter(obj.Value, RobotField.Direction.Left);
                        break;
                    case "RIGHT":
                        result = roboter.IsLetter(obj.Value, RobotField.Direction.Right);
                        break;
                    default:
                        Errors.Add("RUNNING: Condition Letter error");
                        // sollte nicht passieren
                        break;

                }
            }

            return result;
        }
    }
}
