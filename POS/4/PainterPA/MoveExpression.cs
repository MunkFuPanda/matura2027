using System;
using Painter;
using PainterPA;

public class MoveExpression : Expression {
    public int X { get; set; }
    public int Y { get; set; }
    internal override void Parse(List<Token> tokenList) {
        if (tokenList.Count > 1 && tokenList[0].Type == Token.TokenType.NUMBER && tokenList[1].Type == Token.TokenType.NUMBER) {
            if (int.TryParse(tokenList.First().Value, out int x)) {
                X = x;
                tokenList.RemoveAt(0);
            } else {
                Errors.Add($"Zeile {tokenList.First().LineNumber}: Ungültige Zahl: {tokenList.First().Value}");
                tokenList.RemoveAt(0);
            }

            if (int.TryParse(tokenList.First().Value, out int y)) {
                Y = y;
                tokenList.RemoveAt(0);
            } else {
                Errors.Add($"Zeile {tokenList.First().LineNumber}: Ungültige Zahl: {tokenList.First().Value}");
                tokenList.RemoveAt(0);
            }
        } else {
            Errors.Add($"Zeile: {tokenList.First().LineNumber} erwarte Zahl nach MOVE");
        }
    }

    internal override void Run(PainterControl painter) {
        painter.Move(X, Y);
    }
}
