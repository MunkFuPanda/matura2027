using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Painter;
using DataModels;
using LinqToDB;
using System.Text.RegularExpressions;
using LinqToDB.Internal.Common;

namespace PainterPA {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            using var db = new FormenDB(
                    new DataOptions().UseSQLite("Data Source=Formen.db")
            );

            var formen = new List<Forman>();
            var lines = new List<DataModels.Line>();

            formen = db.Formen.ToList();

            SavedList.ItemsSource = formen;
        }

        private void OnRun_Clicked(object sender, RoutedEventArgs e) {
            PainterControl.Clear();
            Expression.Errors.Clear();
            StatusText.Text = "Running";

            Regex regex = new Regex("@LINE|COLOR|FORM|FOR|MOVE|{|}|-?\\d+|[A-Za-z]+|\\S+");
            Regex keywords = new Regex(@"^(LINE|COLOR|FORM|FOR|MOVE)$");

            string[] lines = CodeBox.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            List<Token> tokens = new();

            for (int i = 0; i < lines.Length; i++) {
                MatchCollection matches = regex.Matches(lines[i]);
                foreach (Match match in matches) {
                    Token token = new Token() { Value = match.Value, LineNumber = i + 1 };

                    if (keywords.IsMatch(match.Value))
                        token.Type = Token.TokenType.KEYWORD;
                    else if (match.Value == "{")
                        token.Type = Token.TokenType.OPEN_BRACE;
                    else if (match.Value == "}")
                        token.Type = Token.TokenType.CLOSE_BRACE;
                    else if (Regex.IsMatch(match.Value, @"^-?\d+$"))
                        token.Type = Token.TokenType.NUMBER;
                    else if (Regex.IsMatch(match.Value, @"^[A-Za-z]+$"))
                        token.Type = Token.TokenType.WORD;
                    else
                        token.Type = Token.TokenType.ERROR;

                    tokens.Add(token);
                }
            }


            if (tokens.Any(t => t.Type == Token.TokenType.ERROR)) {
                StringBuilder errorMessage = new();
                foreach (var token in tokens.Where(t => t.Type == Token.TokenType.ERROR)) {
                    errorMessage.AppendLine($"Zeile {token.LineNumber}: Ungültiges Token {token.Value}");
                }

                MessageBox.Show(errorMessage.ToString(), "Tokenisation error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            StatusText.Text = "Parsing";

            Program program = new();
            program.Parse(tokens.Where(t => t.Type != Token.TokenType.ERROR).ToList());

            if (Expression.Errors.Count > 0) {
                StringBuilder errorMessage = new();
                foreach (var error in Expression.Errors) {
                    errorMessage.AppendLine(error);
                }

                MessageBox.Show(errorMessage.ToString(), "Parsing error", MessageBoxButton.OK, MessageBoxImage.Error);
                Expression.Errors.Clear();
            }

            StatusText.Text = "Executing";

            try {
                program.Run(PainterControl);
            } catch (Exception ex) {
                MessageBox.Show($"Fehler: {ex.Message}");
            }

            if (Expression.Errors.Count > 0) {
                StringBuilder errorMessage = new();
                foreach (var error in Expression.Errors) {
                    errorMessage.AppendLine(error);
                }

                MessageBox.Show(errorMessage.ToString(), "Execution error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            StatusText.Text = "Done";
        }

        private void OnSavedList_DoubleClick(object sender, MouseButtonEventArgs e) {

        }
    }
}