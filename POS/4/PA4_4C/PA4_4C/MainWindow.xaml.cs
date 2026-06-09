using DataModels;
using LinqToDB;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
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

namespace PA4_4C
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static MainWindow instance;
        internal List<Worldcity> cityList;
        public MainWindow()
        {
            InitializeComponent();
            instance = this;

            using var db = new WorldcitiesDB(
                    new DataOptions().UseSQLite("Data Source=worldcities.sqlite")
            );

            cityList = db.Worldcities.ToList();

            Code.Text = "COUNTRY Germany {\r\n  RANDOM\r\n  LARGEST\r\n  SMALLEST\r\n}\r\nCOUNTRY Austria {\r\n  LARGEST\r\n  SMALLEST\r\n}\r\nCOUNTRY France {\r\n  RANDOM\r\n  RANDOM\r\n  RANDOM\r\n}";

            //Aufgabe 7
            //Code.Text = "COUNTRY Austria {\r\n  SELECT\r\n}\r\nCOUNTRY Germany {\r\n  LARGEST\r\n  SMALLEST\r\n}\r\nCOUNTRY France {\r\n  RANDOM\r\n  RANDOM\r\n  RANDOM\r\n}\r\nCOUNTRY Spain {\r\n  LARGEST\r\n  SMALLEST\r\n}";
        }

        /*public static Worldcity CityDialog()
        {
            instance.ShowDialog();
        }*/

        private void OnRun_Clicked(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"COUNTRY|RANDOM|LARGEST|SMALLEST|{|}|[A-Za-z]+|\\S+");
            Regex keywords = new Regex(@"^(COUNTRY|RANDOM|LARGEST|SMALLEST)$");

            string[] lines = Code.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            List<Token> tokens = new();

            for (int i = 0; i < lines.Length; i++)
            {
                MatchCollection matches = regex.Matches(lines[i]);
                foreach (Match match in matches)
                {
                    Token token = new Token() { Value = match.Value };

                    if (keywords.IsMatch(match.Value))
                        token.Type = Token.TokenType.Keyword;
                    else if (match.Value == "{")
                        token.Type = Token.TokenType.Open;
                    else if (match.Value == "}")
                        token.Type = Token.TokenType.Close;
                    else if (Regex.IsMatch(match.Value, @"^[A-Za-z]+$"))
                        token.Type = Token.TokenType.Identifier;
                    else
                        token.Type = Token.TokenType.Error;

                    tokens.Add(token);
                }
            }


            if (tokens.Any(t => t.Type == Token.TokenType.Error))
            {
                StringBuilder errorMessage = new();
                foreach (var token in tokens.Where(t => t.Type == Token.TokenType.Error))
                {
                    errorMessage.AppendLine($"Ungültiges Token {token.Value}");
                }

                MessageBox.Show(errorMessage.ToString(), "Tokenisation error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Program program = new();
            program.Parse(tokens.Where(t => t.Type != Token.TokenType.Error).ToList());

            if (Expression.Errors.Count > 0)
            {
                StringBuilder errorMessage = new();
                foreach (var error in Expression.Errors)
                {
                    errorMessage.AppendLine(error);
                }

                MessageBox.Show(errorMessage.ToString(), "Parsing error", MessageBoxButton.OK, MessageBoxImage.Error);
                Expression.Errors.Clear();
            }

            List<Worldcity> results = [];

            try
            {
                program.Run(cityList, results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}");
            }

            if (Expression.Errors.Count > 0)
            {
                StringBuilder errorMessage = new();
                foreach (var error in Expression.Errors)
                {
                    errorMessage.AppendLine(error);
                }

                MessageBox.Show(errorMessage.ToString(), "Execution error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MessageBox.Show(results.Count.ToString(), "selected ctities", MessageBoxButton.OK, MessageBoxImage.None);
        }

    }
}