namespace WPF_Painter
{
    internal static class Parser
    {
        public static Expression ParseNext(List<Token> tokens)
        {
            if (tokens.Count == 0) return null;

            string command = tokens[0].Value.ToUpper();
            tokens.RemoveAt(0); // Das Schlüsselwort (z.B. "DRAW") entfernen

            Expression expr = null;

            switch (command)
            {
                case "DRAW":
                    expr = new DrawExpression();
                    break;
                case "TURN":
                    expr = new TurnExpression();
                    break;
                case "COLOR":
                    expr = new ColorExpression();
                    break;
                case "FOR":
                    expr = new ForExpression();
                    break;
                default:
                    Expression.Errors.Add($"Unbekannter Befehl: {command}");
                    return null;
            }

            expr.Parse(tokens);
            return expr;
        }
    }
}