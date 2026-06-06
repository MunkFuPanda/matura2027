namespace WPF_Painter
{
    internal class ForExpression : Expression
    {
        public int Count { get; set; }
        public List<Expression> Body { get; set; } = new List<Expression>();
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.NUMBER)
            {
                if (int.TryParse(tokens[0].Value, out int count))
                {
                    Count = count;
                    tokens.RemoveAt(0); // Entferne die Anzahlangabe
                }
                else
                {
                    Errors.Add($"Zeile {tokens[0].LineNumber}: Ungültige Zahl: {tokens[0].Value}");
                    tokens.RemoveAt(0); // Entferne das ungültige Token, um die Analyse fortzusetzen
                }
            }
            else
            {
                Errors.Add("Erwartet: Zahl nach FOR");
                return; // Keine gültige Anzahl, daher Abbruch der Analyse
            }
            if (tokens.Count > 0)
            {
                if (tokens[0].Type == Token.TokenType.OPEN_BRACE)
                {
                    tokens.RemoveAt(0); // Entferne die öffnende Klammer
                }
                else
                {
                    string foundType = tokens[0].Type == Token.TokenType.KEYWORD ? "Keyword" :
                                       (tokens[0].Type == Token.TokenType.NUMBER ? "Number" :
                                       (tokens[0].Type == Token.TokenType.WORD ? "Color" : tokens[0].Type.ToString()));
                    Errors.Add($"Zeile {tokens[0].LineNumber}: Incorrect Block Statement, exptecting {{ and found {foundType}: {tokens[0].Value}");
                }
            }
            // Analysiere den Block innerhalb der geschweiften Klammern
            while (tokens.Count > 0)
            {
                Token token = tokens.First();
                if (token.Type == Token.TokenType.CLOSE_BRACE)
                {
                    tokens.RemoveAt(0); // Entferne die schließende Klammer
                    break; // Ende des Blocks erreicht
                }
                else if (token.Type == Token.TokenType.KEYWORD)
                {
                    switch (token.Value)
                    {
                        case "TURN":
                            TurnExpression turnExpr = new TurnExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "TURN"
                            turnExpr.Parse(tokens);
                            Body.Add(turnExpr);
                            break;
                        case "COLOR":
                            ColorExpression colorExpr = new ColorExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "COLOR"
                            colorExpr.Parse(tokens);
                            Body.Add(colorExpr);
                            break;
                        case "DRAW":
                            DrawExpression drawExpr = new DrawExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "DRAW"
                            drawExpr.Parse(tokens);
                            Body.Add(drawExpr);
                            break;
                        case "FOR":
                            ForExpression forExpr = new ForExpression();
                            tokens.RemoveAt(0); // Entferne das Schlüsselwort "FOR"
                            forExpr.Parse(tokens);
                            Body.Add(forExpr);
                            break;
                        default:
                            Errors.Add($"Zeile {token.LineNumber}: Unbekanntes Schlüsselwort: {token.Value}");
                            tokens.RemoveAt(0); // Entferne das unbekannte Schlüsselwort
                            break;
                    }
                }
                else
                {
                    string foundType = token.Type == Token.TokenType.KEYWORD ? "Keyword" :
                                       (token.Type == Token.TokenType.NUMBER ? "Number" :
                                       (token.Type == Token.TokenType.WORD ? "Color" : token.Type.ToString()));
                    Errors.Add($"Zeile {token.LineNumber}: Unexpected Token, expected Keyword, found {foundType}: {token.Value}");
                    tokens.RemoveAt(0); // Entferne das unerwartete Token, um die Analyse fortzusetzen
                }
            }
        }
        internal override void Execute(Painter.PainterControl painter)
        {
            for (int i = 0; i < Count; i++)
            {
                foreach (var expr in Body)
                {
                    expr.Execute(painter);
                }
            }
        }
    }
}