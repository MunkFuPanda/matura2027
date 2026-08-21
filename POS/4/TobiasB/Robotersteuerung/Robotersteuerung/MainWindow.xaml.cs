using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Robotersteuerung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // field loading here
            RobotFieldControl.LoadField("Aufgabe1.xml");
        }

        private void Tokenize_Click(object sender, RoutedEventArgs e)
        {
            // Schritt 1: Tokenisierung

            // https://regex101.com/
            Regex regex = new Regex(@"REPEAT|MOVE|RIGHT|DOWN|LEFT|UP|COLLECT|{|}|UNTIL|IS-A|OBSTACLE|IF|[A-Z]|\d+|\S+");
            Regex keywords = new Regex(@"REPEAT|MOVE|RIGHT|DOWN|LEFT|UP|COLLECT|UNTIL|IS-A|OBSTACLE|IF");
            MatchCollection matches = regex.Matches(tb_input.Text);
            List<Token> tokens = new List<Token>();

            foreach (Match match in matches)
            {
                Token token = new Token() { Value = match.Value };

                if (keywords.IsMatch(match.Value))
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
                else if (Regex.IsMatch(match.Value, @"[A-Z]"))
                {
                    token.Type = Token.TokenType.LETTER;
                }
                else if (Regex.IsMatch(match.Value, @"\d+"))
                {
                    token.Type = Token.TokenType.NUMBER;
                }
                else
                {
                    token.Type = Token.TokenType.ERROR;
                }

                tokens.Add(token);


            }

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                TokensListBox.ItemsSource = tokens;
            }));

            // Schritt 1.5 Ausgabe Fehler falls vorhanden beim Tokenizen

            if (tokens.Any(t => t.Type == Token.TokenType.ERROR))
            {
                StringBuilder sb = new StringBuilder();
                foreach (Token token in tokens.Where(t => t.Type == Token.TokenType.ERROR))
                {
                    sb.AppendLine(token.Value);
                    
                }
                MessageBox.Show(sb.ToString(), "Fehler beim Tokenizen", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            // Schritt 2: Parsing
            Program program = new Program();
            program.Parse(tokens.Where(x => x.Type != Token.TokenType.ERROR).ToList());

            // Schritt 2.5: Ausgabe Fehler falls vorhanden beim Parsen

            if (Expression.Errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var error in Expression.Errors)
                {
                    sb.AppendLine(error);
                }
                MessageBox.Show(sb.ToString(), "Fehler beim Parsen", MessageBoxButton.OK, MessageBoxImage.Error);
                Expression.Errors.Clear();
            }

            // Schritt 3: Executing 

            ThreadPool.QueueUserWorkItem(_ =>
            {
                program.Execute(RobotFieldControl);

                // Schritt 3.5: Ausgabe Fehler falls vorhanden beim Ausführen
                // im Thread weil es sonst nebenbei macht

                if (Expression.Errors.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var error in Expression.Errors)
                    {
                        sb.AppendLine(error);
                    }
                    MessageBox.Show(sb.ToString(), "Fehler beim Ausführen", MessageBoxButton.OK, MessageBoxImage.Error);
                    Expression.Errors.Clear();
                }
            });

            

            


        }
    }
}