using Painter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Painter
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
                        case "TURN":
                            TurnExpression turnExpr = new TurnExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "TURN"
                            turnExpr.Parse(tokens);
                            expressions.Add(turnExpr);
                            break;
                        case "COLOR":
                            ColorExpression colorExpr = new ColorExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "COLOR"
                            colorExpr.Parse(tokens);
                            expressions.Add(colorExpr);
                            break;
                        case "DRAW":
                            DrawExpression drawExpr = new DrawExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "DRAW"
                            drawExpr.Parse(tokens);
                            expressions.Add(drawExpr);
                            break;
                        case "FOR":
                            ForExpression forExpr = new ForExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "FOR"
                            forExpr.Parse(tokens);
                            expressions.Add(forExpr);
                            break;
                        default:
                            Errors.Add($"Zeile {token.LineNumber}: Unbekanntes Schlüsselwort: {token.Value}");
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
                    string foundType = token.Type == Token.TokenType.KEYWORD ? "Keyword" :
                                       (token.Type == Token.TokenType.NUMBER ? "Number" :
                                       (token.Type == Token.TokenType.WORD ? "Color" : token.Type.ToString()));
                    Errors.Add($"Zeile {token.LineNumber}: Unexpected Token, expected Keyword, found {foundType}: {token.Value}");
                    tokens.RemoveAt(0); // Entferne das unbekannte Token, um die Analyse fortzusetzen
                }
            }
        }

        internal override void Execute(Painter.PainterControl roboter)
        {
            foreach (Expression expression in expressions)
            {
                expression.Execute(roboter);
            }
        }
    }
}
