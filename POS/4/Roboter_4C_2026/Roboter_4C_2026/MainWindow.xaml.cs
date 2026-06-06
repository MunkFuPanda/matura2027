using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Roboter_4C_2026
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RobotFieldControl.LoadField("Aufgabe2.xml");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Schritt 1: Tokenisierung
            Regex regex = new Regex(@"IS-A|OBSTACLE|IF|REPEAT|UNTIL|MOVE|RIGHT|DOWN|LEFT|UP|COLLECT|{|}|\d+|[A-Z]|\S+");
            Regex keywords = new Regex(@"IS-A|OBSTACLE|IF|REPEAT|UNTIL|MOVE|RIGHT|DOWN|LEFT|UP|COLLECT");
            MatchCollection matches = regex.Matches(Input.Text);
            List<Token> tokens = new List<Token>();
            foreach (Match match in matches) 
            { 
                Token token = new Token() { Value = match.Value };

                if (keywords.IsMatch(match.Value)) 
                    token.Type = Token.TokenType.KEYWORD;
                else if (match.Value == "{") 
                    token.Type = Token.TokenType.OPEN_BRACE;
                else if (match.Value == "}") 
                    token.Type = Token.TokenType.CLOSE_BRACE;
                else if (Regex.IsMatch(match.Value, @"\d+")) 
                    token.Type = Token.TokenType.NUMBER;
                else if (Regex.IsMatch(match.Value, @"[A-Z]")) 
                    token.Type = Token.TokenType.LETTER;
                else
                {
                    token.Type = Token.TokenType.ERROR;
                }

                    tokens.Add(token);
            }
            TokensListBox.ItemsSource = tokens;

            //Schritt 1.5: Ausgabe der Fehler falls vorhanden
            if (tokens.Any(t => t.Type == Token.TokenType.ERROR)) 
            {
                StringBuilder errorMessage = new StringBuilder();
                foreach (var token in tokens.Where(t => t.Type == Token.TokenType.ERROR))
                {
                    errorMessage.AppendLine($"Ungültiges Token: {token.Value}");
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
            }

            // Schritt 3: Ausführung

            ThreadPool.QueueUserWorkItem(_ => 
            {
                try
                {
                    program.Execute(RobotFieldControl);
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => MessageBox.Show($"Fehler: {ex.Message}"));
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
            });

            //Schritt 4:

        }
    }
}