using System.Text.RegularExpressions;
using System.Windows;

namespace WPF_Painter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Schritt 0: Fehlerliste leeren
            Expression.Errors.Clear();

            // Schritt 1: Tokenisierung (Regex angepasst auf Painter-Sprache)
            // Erkennt Keywords, Klammern, Zahlen und Farbnamen/Strings
            Regex regex = new Regex(@"FOR|DRAW|TURN|COLOR|LEFT|RIGHT|{|}|\d+|[A-Z]+|\S+");
            Regex keywords = new Regex(@"FOR|DRAW|TURN|COLOR|RIGHT|LEFT");
            Regex directions = new Regex(@"LEFT|RIGHT");

            MatchCollection matches = regex.Matches(Input.Text);
            List<Token> tokens = new List<Token>();

            foreach (Match match in matches)
            {
                Token token = new Token() { Value = match.Value };

                if (keywords.IsMatch(match.Value.ToUpper()))
                {
                    token.Type = Token.TokenType.KEYWORD;
                }
                else if (match.Value == "{")
                {
                    token.Type = Token.TokenType.OPEN_BRACE;
                }
                else if (match.Value == "}")
                {
                    token.Type = Token.TokenType.CLOSE_BRACE;
                }
                else if (Regex.IsMatch(match.Value, @"^\d+$"))
                {
                    token.Type = Token.TokenType.NUMBER;
                }
                else if (Regex.IsMatch(match.Value, @"^[a-zA-Z]+$"))
                {
                    // Farben wie "Red" landen hier als LETTER, 
                    // da Schermann glaub ich meinte, wir sollen die einfach als
                    // strings speichern und dann umtokenizen.
                    token.Type = Token.TokenType.LETTER;
                }
                else
                {
                    token.Type = Token.TokenType.ERROR;
                }
                tokens.Add(token);
            }

            // Schritt 1.5: Zeichen Fehler prüfen
            if (tokens.Any(t => t.Type == Token.TokenType.ERROR))
            {
                var errors = tokens.Where(t => t.Type == Token.TokenType.ERROR);
                MessageBox.Show("Ungültige Zeichen gefunden: " + string.Join(", ", errors.Select(t => t.Value)));
                return;
            }

            // Schritt 2: Parsing
            Block rootProgram = new();
            // Da ein Programm im Grunde nur ein großer Block ohne Klammern ist, 
            // mache ich mir die Arbeit einfach indem ich
            // eine extra "Programm"-Klasse nutze.
            // Einfacher: ich parse, solange Tokens da sind.

            List<Expression> expressions = new List<Expression>();
            while (tokens.Count > 0)
            {
                var expr = Parser.ParseNext(tokens);
                if (expr != null) expressions.Add(expr);
            }

            if (Expression.Errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", Expression.Errors), "Fehler beim Parsen");
                return;
            }

            // Schritt 3: Ausführen
            // Erstellung des Kontexts mit dem Painter-Control aus dem XAML
            PainterContext context = new PainterContext(this.MyPainterControl);

            // Painter zurücksetzen (falls die .dll eine Clear-Methode hat, wie im Bild)
            MyPainterControl.Clear();

            foreach (var expr in expressions)
            {
                expr.Interpret(context);
            }
        }
    }
}