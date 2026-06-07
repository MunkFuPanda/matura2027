using System;
using LinqToDB.Linq;
using Painter;
using PainterPA;

public class ForExpression : Expression {
    public int Count { get; set; }
    public List<Expression> Body { get; set; } = [];
    internal override void Parse(List<Token> tokenList) {
        if (tokenList.Count > 0 && tokenList.First().Type == Token.TokenType.NUMBER) {
            Count = int.Parse(tokenList.First().Value);
            tokenList.RemoveAt(0);

            if (tokenList.Count > 0) {
                if (tokenList.First().Type == Token.TokenType.OPEN_BRACE) {
                    tokenList.RemoveAt(0);
                    while (tokenList.Count > 0 && tokenList.First().Type != Token.TokenType.CLOSE_BRACE) {
                        if (tokenList.First().Type != Token.TokenType.KEYWORD) {
                            string foundType = tokenList.First().Type switch {
                                Token.TokenType.KEYWORD => "Keyword",
                                Token.TokenType.NUMBER => "Number",
                                Token.TokenType.WORD => "Color",
                                _ => tokenList.First().Type.ToString()
                            };

                            Errors.Add(
                                $"Zeile {tokenList.First().LineNumber}: Unexpected Token, expected Keyword, found {foundType}: {tokenList.First().Value}");

                            tokenList.RemoveAt(0);
                            continue;
                        }

                        if (!ExpressionFactories.TryGetValue(tokenList.First().Value, out var factory)) {
                            Errors.Add(
                                $"Zeile {tokenList.First().LineNumber}: Unbekanntes Schlüsselwort: {tokenList.First().Value}");

                            tokenList.RemoveAt(0);
                            continue;
                        }

                        tokenList.RemoveAt(0); // Keyword konsumieren

                        Expression expression = factory();
                        expression.Parse(tokenList);
                        Body.Add(expression);
                    }
                    if (tokenList.Count > 0 && tokenList.First().Type == Token.TokenType.CLOSE_BRACE) {
                        tokenList.RemoveAt(0);
                    } else {
                        Errors.Add($"Zeile {tokenList.First().LineNumber}: Fehlende schließende Klammer für For-Schleife");
                    }
                } else {
                    Errors.Add($"Zeile {tokenList.First().LineNumber}: Erwartete öffnende Klammer für For-Schleife, nicht {tokenList.First().Value}");
                    tokenList.RemoveAt(0);
                }
            }
        } else {
            Errors.Add($"Zeile {tokenList.First().LineNumber}: Erwartete Zahl für For-Schleife, nicht {tokenList.First().Value}");
            tokenList.RemoveAt(0);
        }
    }

    internal override void Run(PainterControl painter) {
        for (int i = 0; i < Count; i++) {
            foreach (var expression in Body) {
                expression.Run(painter);
            }
        }
    }
}
