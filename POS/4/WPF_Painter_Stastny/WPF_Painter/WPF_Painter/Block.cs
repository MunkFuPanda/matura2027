namespace WPF_Painter
{
    internal class Block : Expression
    {
        private List<Expression> expressions = new List<Expression>();

        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens[0].Value == "{")
            {
                tokens.RemoveAt(0); // '{' entfernen

                while (tokens.Count > 0 && tokens[0].Value != "}")
                {
                    // Der Parser entscheidet anhand des Tokens, welche Expression erstellt wird
                    Expression next = Parser.ParseNext(tokens);
                    if (next != null) expressions.Add(next);
                }

                if (tokens.Count > 0 && tokens[0].Value == "}")
                {
                    tokens.RemoveAt(0); // '}' entfernen
                }
                else
                {
                    Errors.Add("Fehler: Schließende Klammer '}' fehlt.");
                }
            }
            else
            {
                Errors.Add("Fehler: Block muss mit '{' beginnen.");
            }
        }

        internal override void Interpret(PainterContext context)
        {
            foreach (var expr in expressions)
            {
                expr.Interpret(context);
            }
        }
    }
}