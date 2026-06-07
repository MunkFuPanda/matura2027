using System;
using Painter;
using PainterPA;

public class ColorExpression : Expression {
    public string Color { get; set; }
    internal override void Parse(List<Token> tokenList) {
        if (tokenList.Count > 0) {
            if (tokenList[0].Type == Token.TokenType.WORD) {
                Color = tokenList[0].Value;
                tokenList.RemoveAt(0);
            } else {
                string foundType = tokenList[0].Type == Token.TokenType.KEYWORD ? "Keyword" :
                                   (tokenList[0].Type == Token.TokenType.NUMBER ? "Number" :
                                   (tokenList[0].Type == Token.TokenType.WORD ? "Color" : tokenList[0].Type.ToString()));
                Errors.Add($"Zeile {tokenList[0].LineNumber}: Incorrect Color Statement, exptecting Colorname and found {foundType}: {tokenList[0].Value}");
            }
        } else {
            Errors.Add("Erwartet: Farbangabe nach COLOR");
        }
    }

    internal override void Run(PainterControl painter) {
        if (!string.IsNullOrEmpty(Color)) {
            painter.ChangeColor(Color);
        }
    }
}
