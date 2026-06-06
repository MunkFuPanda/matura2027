using AbcRobotCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboter_4C_2026
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
                    switch(token.Value)
                    {
                        case "MOVE":
                            MoveExpression moveExpr = new MoveExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "MOVE"
                            moveExpr.Parse(tokens);
                            expressions.Add(moveExpr);
                            break;
                        case "COLLECT":
                            CollectExpression collectExpr = new CollectExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "COLLECT"
                            collectExpr.Parse(tokens);
                            expressions.Add(collectExpr);
                            break;
                        case "REPEAT":
                            RepeatExpression repeatExpr = new RepeatExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "REPEAT"
                            repeatExpr.Parse(tokens);
                            expressions.Add(repeatExpr);
                            break;
                        case "IF":
                            IfExpression ifExpr = new IfExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "IF"
                            ifExpr.Parse(tokens);
                            expressions.Add(ifExpr);
                            break;
                        case "UNTIL":
                            UntilExpression untilExpr = new UntilExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "UNTIL"
                            untilExpr.Parse(tokens);
                            expressions.Add(untilExpr);
                            break;
                        default:
                            Errors.Add($"Unbekanntes Schlüsselwort: {token.Value}");
                            tokens.RemoveAt(0); // Entferne das unbekannte Token, um die Analyse fortzusetzen
                            break;
                    }
                }
                else
                {
                    if (token.Type == Token.TokenType.CLOSE_BRACE)
                    {
                        return; // Ende des aktuellen Blocks erreicht
                    }
                    Errors.Add($"Unerwartetes Token: {token.Value}");
                    tokens.RemoveAt(0); // Entferne das unbekannte Token, um die Analyse fortzusetzen
                }
            }
        }

        internal override void Execute(RobotField roboter)
        {
            foreach (Expression expression in expressions)
            {
                expression.Execute(roboter);
            }
        }
    }
}
