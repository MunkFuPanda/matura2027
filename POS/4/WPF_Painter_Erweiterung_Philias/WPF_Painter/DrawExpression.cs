namespace WPF_Painter
{
    internal class DrawExpression : Expression
    {
        public int Distance { get; set; }
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Type == Token.TokenType.NUMBER)
            {
                if (int.TryParse(tokens[0].Value, out int distance))
                {
                    Distance = distance;
                    tokens.RemoveAt(0); // Entferne die Distanzangabe
                }
                else
                {
                    Errors.Add($"Zeile {tokens[0].LineNumber}: Ungültige Zahl: {tokens[0].Value}");
                    tokens.RemoveAt(0); // Entferne das ungültige Token, um die Analyse fortzusetzen
                }
            }
            else
            {
                Errors.Add("Erwartet: Zahl nach DRAW");
            }
        }
        
        internal override void Execute(Painter.PainterControl painter)
        {
            painter.Draw(Distance);
        }
    }
}