using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
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

            System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader("Paint.xshd");
            tb_input.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Schritt 1: Tokenisierung - includes newlines to track line numbers

            // hier geht die Zeileninformation verloren, darum erhalten -> im Token abspeichern
            Regex regex = new Regex(@"DRAW|TURN|COLOR|FOR|RIGHT|LEFT|UP|DOWN|{|}|\d+|\w+|\n");
            Regex keywords = new Regex(@"DRAW|TURN|COLOR|FOR|RIGHT|LEFT|UP|DOWN");
            MatchCollection matchCollection = regex.Matches(tb_input.Text);
            List<Token> tokens = new List<Token>();
            int currentLine = 1;

            foreach (Match match in matchCollection)
            {
                Token token = new Token() { Value = match.Value, LineNumber = currentLine };

                if (match.Value == "\n")
                {
                    // muss nicht neu erstellt werden
                    token.Type = Token.TokenType.NEWLINE;
                    currentLine++;
                }
                else if (keywords.IsMatch(match.Value))
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
                    token.Type = Token.TokenType.LETTERS;
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

            

            if (tokens.Any(t => t.Type == Token.TokenType.ERROR))
            {
                StringBuilder sb = new StringBuilder();
                foreach (Token token in tokens.Where(t => t.Type == Token.TokenType.ERROR))
                {
                    sb.AppendLine(token.Value);

                }
                MessageBox.Show(sb.ToString(), "Fehler beim Tokenizen", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Program program = new Program();

            program.Parse(tokens);

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

            ThreadPool.QueueUserWorkItem(_ =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    program.Execute(painterGrid);
                });
                

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