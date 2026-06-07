using System;
using Painter;
using PainterPA;

public class Program : Expression
{
    private List<Expression> expressions = [];

    internal override void Parse(List<Token> tokenList) {
        while (tokenList.Count > 0) {
            var token = tokenList.First();
            if (token.Type == Token.TokenType.CLOSE_BRACE)
                return;

            if (token.Type != Token.TokenType.KEYWORD) {
                string foundType = token.Type switch {
                    Token.TokenType.KEYWORD => "Keyword",
                    Token.TokenType.NUMBER => "Number",
                    Token.TokenType.WORD => "Color",
                    _ => token.Type.ToString()
                };

                Errors.Add(
                    $"Zeile {token.LineNumber}: Unexpected Token, expected Keyword, found {foundType}: {token.Value}");

                tokenList.RemoveAt(0);
                continue;
            }

            if (!ExpressionFactories.TryGetValue(token.Value, out var factory)) {
                Errors.Add(
                    $"Zeile {token.LineNumber}: Unbekanntes Schlüsselwort: {token.Value}");

                tokenList.RemoveAt(0);
                continue;
            }

            tokenList.RemoveAt(0); // Keyword konsumieren

            Expression expression = factory();
            expression.Parse(tokenList);
            expressions.Add(expression);
        }
    }

    internal override void Run(PainterControl painter) {
        foreach (Expression expression in expressions)
            expression.Run(painter);
    }
}
