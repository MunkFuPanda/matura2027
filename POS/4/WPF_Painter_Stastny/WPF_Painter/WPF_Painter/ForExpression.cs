namespace WPF_Painter
{
    internal class ForExpression : Expression
    {
        private int count = 0;
        private Block block;

        internal override void Parse(List<Token> tokens)
        {
            // 1. Anzahl der Wiederholungen lesen
            if (tokens.Count > 0 && int.TryParse(tokens[0].Value, out count))
            {
                tokens.RemoveAt(0);
            }
            else
            {
                Errors.Add("Fehler: FOR erwartet eine Zahl, erhalten: " +
                           (tokens.Count > 0 ? tokens[0].Value : "Ende der Eingabe"));
                return;
            }

            // 2. Den Block initialisieren und parsen (verarbeitet { ... })
            block = new Block();
            block.Parse(tokens);
        }

        internal override void Interpret(PainterContext context)
        {
            for (int i = 0; i < count; i++)
            {
                // Führt den gesamten Block wiederholt aus
                block?.Interpret(context);
            }
        }
    }
}