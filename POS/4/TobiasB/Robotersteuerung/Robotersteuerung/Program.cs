using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotersteuerung
{
    internal class Program : Expression
    {
        private List<Expression> expressions = new List<Expression>();

        internal override void Parse(List<Token> tokens)
        {
            while (tokens.Count > 0)
            {
                Token token = tokens.First();
                
                if (token.Type == Token.TokenType.KEYWORD)
                {
                    switch (token.Value)
                    {
                        case "MOVE":
                            MoveExpression moveExpression = new MoveExpression();
                            tokens.RemoveAt(0); // damit der Token wegfällt den wir gerade verarbeiten
                            moveExpression.Parse(tokens);
                            expressions.Add(moveExpression);
                            break;
                        case "COLLECT":
                            CollectExpression collectExpression = new CollectExpression();
                            tokens.RemoveAt(0); // damit der Token wegfällt den wir gerade verarbeiten
                            expressions.Add(collectExpression);
                            break;
                        case "REPEAT":
                            RepeatExpression repeatExpression = new RepeatExpression();
                            tokens.RemoveAt(0); // damit der Token wegfällt den wir gerade verarbeiten
                            repeatExpression.Parse(tokens);
                            expressions.Add(repeatExpression);
                            break;
                        case "UNTIL":
                            UntilExpression untilExpression = new UntilExpression();
                            tokens.RemoveAt(0); // damit der Token wegfällt den wir gerade verarbeiten
                            untilExpression.Parse(tokens);
                            expressions.Add(untilExpression);
                            break;
                        case "IF":
                            IfExpression ifExpression = new IfExpression();
                            tokens.RemoveAt(0); // damit der Token wegfällt den wir gerade verarbeiten
                            ifExpression.Parse(tokens);
                            expressions.Add(ifExpression);
                            break;
                        default:
                            Errors.Add($"Unerwartetes Schlüsselwort: {token.Value}");
                            tokens.RemoveAt(0);
                            break;
                    }
                }
                else
                {
                    if (token.Type == Token.TokenType.CLOSE_BRACE)
                    {
                        // Programm ist fertig
                        return;
                    }
                    Errors.Add($"Unerwartetes Token: {token.Value}");
                    tokens.RemoveAt(0); //Damit das weiterläuft wie oben im switch
                }
            }
            
        }

        internal override void Execute(RobotField roboter)
        {
            foreach(Expression expression in expressions)
            {
                expression.Execute(roboter);
            }
        }

    }
}
