using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace WPF_Painter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PainterControl.Clear();
            Expression.Errors.Clear();
            // Schritt 1: Tokenisierung
            Regex regex = new Regex(@"TURN|COLOR|DRAW|FOR|RIGHT|LEFT|{|}|\d+|[A-Za-z]+|\S+");
            Regex keywords = new Regex(@"^(TURN|COLOR|DRAW|FOR|RIGHT|LEFT)$");

            string[] lines = InputTextBox.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            List<Token> tokens = new List<Token>();

            for (int i = 0; i < lines.Length; i++)
            {
                MatchCollection matches = regex.Matches(lines[i]);
                foreach (Match match in matches)
                {
                    Token token = new Token() { Value = match.Value, LineNumber = i + 1 };

                    if (keywords.IsMatch(match.Value))
                        token.Type = Token.TokenType.KEYWORD;
                    else if (match.Value == "{")
                        token.Type = Token.TokenType.OPEN_BRACE;
                    else if (match.Value == "}")
                        token.Type = Token.TokenType.CLOSE_BRACE;
                    else if (Regex.IsMatch(match.Value, @"^\d+$"))
                        token.Type = Token.TokenType.NUMBER;
                    else if (Regex.IsMatch(match.Value, @"^[A-Za-z]+$"))
                        token.Type = Token.TokenType.WORD;
                    else
                        token.Type = Token.TokenType.ERROR;

                    tokens.Add(token);
                }
            }

            //Schritt 1.5: Ausgabe der Fehler falls vorhanden
            if (tokens.Any(t => t.Type == Token.TokenType.ERROR))
            {
                StringBuilder errorMessage = new StringBuilder();
                foreach (var token in tokens.Where(t => t.Type == Token.TokenType.ERROR))
                {
                    errorMessage.AppendLine($"Zeile {token.LineNumber}: Ungültiges Token: {token.Value}");
                }
                MessageBox.Show(errorMessage.ToString(), "Tokenisierungs Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Schritt 2: Parsing
            Program program = new Program();
            program.Parse(tokens.Where(t => t.Type != Token.TokenType.ERROR).ToList());

            // Schritt 2.5: Ausgabe der Fehler falls vorhanden

            if (Expression.Errors.Count > 0)
            {
                StringBuilder errorMessage = new StringBuilder();
                foreach (var error in Expression.Errors)
                {
                    errorMessage.AppendLine(error);
                }
                MessageBox.Show(errorMessage.ToString(), "Parsing Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                Expression.Errors.Clear();
            }

            // Schritt 3: Ausführung
            try
            {
                // Start-Position setzen
                PainterControl.Rotate(90);
                PainterControl.ChangeColor("White");
                PainterControl.Draw(130);
                PainterControl.Rotate(-90);
                PainterControl.Draw(150);
                PainterControl.ChangeColor("Black");

                program.Execute(PainterControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}");
            }

            //Schritt 3.5: Ausgabe Fehler falls vorhanden
            if (Expression.Errors.Count > 0)
            {
                StringBuilder errorMessage = new StringBuilder();
                foreach (var error in Expression.Errors)
                {
                    errorMessage.AppendLine(error);
                }
                MessageBox.Show(errorMessage.ToString(), "Ausführungs Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}