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

                if (token.Type == Token.TokenType.NEWLINE)
                {
                    tokens.RemoveAt(0);
                    continue;
                }

                if (token.Type == Token.TokenType.KEYWORD)
                {
                    switch (token.Value)
                    {
                        case "DRAW":
                            DrawExpression drawExpression = new DrawExpression(token.LineNumber);
                            tokens.RemoveAt(0);
                            drawExpression.Parse(tokens);
                            expressions.Add(drawExpression);
                            break;
                        case "TURN":
                            TurnExpression turnExpression = new TurnExpression(token.LineNumber);
                            tokens.RemoveAt(0);
                            turnExpression.Parse(tokens);
                            expressions.Add(turnExpression);
                            break;
                        case "COLOR":
                            ColorExpression colorExpression = new ColorExpression(token.LineNumber);
                            tokens.RemoveAt(0);
                            colorExpression.Parse(tokens);
                            expressions.Add(colorExpression);
                            break;
                        case "FOR":
                            ForExpression forExpression = new ForExpression(token.LineNumber);
                            tokens.RemoveAt(0);
                            forExpression.Parse(tokens);
                            expressions.Add(forExpression);
                            break;
                        default:
                            Errors.Add($"Zeile {token.LineNumber}: Unerwartetes Schlüsselwort: {token.Value}");
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
                    Errors.Add($"Zeile {token.LineNumber}: Unerwartetes Token: {token.Value}");
                    tokens.RemoveAt(0);
                }
            }
        }

        internal override void Execute(PainterControl painterControl)
        {
            foreach (Expression expression in expressions)
            {
                expression.Execute(painterControl);
            }
        }
    }
}
