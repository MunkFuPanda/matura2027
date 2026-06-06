namespace WPF_Painter
{
    internal class ColorExpression : Expression
    {
        public string Color { get; set; }

        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0)
            {
                if (tokens[0].Type == Token.TokenType.WORD)
                {
                    Color = tokens[0].Value;
                    tokens.RemoveAt(0); // Entferne die Farbangabe
                }
                else
                {
                    string foundType = tokens[0].Type == Token.TokenType.KEYWORD ? "Keyword" :
                                       (tokens[0].Type == Token.TokenType.NUMBER ? "Number" :
                                       (tokens[0].Type == Token.TokenType.WORD ? "Color" : tokens[0].Type.ToString()));
                    Errors.Add($"Zeile {tokens[0].LineNumber}: Incorrect Color Statement, exptecting Colorname and found {foundType}: {tokens[0].Value}");
                }
            }
            else
            {
                Errors.Add("Erwartet: Farbangabe nach COLOR");
            }
        }
        
        internal override void Execute(Painter.PainterControl painter)
        {
            if (!string.IsNullOrEmpty(Color))
            {
                painter.ChangeColor(Color);
            }
        }
    }
}