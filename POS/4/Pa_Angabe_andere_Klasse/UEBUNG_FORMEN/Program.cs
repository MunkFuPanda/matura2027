using Painter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UEBUNG_FORMEN
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
                            MoveExpression moveExpr = new MoveExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "MOVE"
                            moveExpr.Parse(tokens);
                            expressions.Add(moveExpr);
                            break;
                        case "COLOR":
                            ColorExpression colorExpr = new ColorExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "COLOR"
                            colorExpr.Parse(tokens);
                            expressions.Add(colorExpr);
                            break;
                        case "LINE":
                            LineExpression lineExpr = new LineExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "LINE"
                            lineExpr.Parse(tokens);
                            expressions.Add(lineExpr);
                            break;
                        case "FOR":
                            ForExpression forExpr = new ForExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "FOR"
                            forExpr.Parse(tokens);
                            expressions.Add(forExpr);
                            break;
                        case "FORM":
                            FormExpression formExpr = new FormExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "FORM"
                            formExpr.Parse(tokens);
                            expressions.Add(formExpr);
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

        internal override void Run(Painter.PainterControl roboter)
        {
            foreach (Expression expression in expressions)
            {
                expression.Run(roboter);
            }
        }
    }
}
