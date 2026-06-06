using System.IO;
using System.Text;
using System.Windows;
using Robotersteuerung.Parser;
using Robotersteuerung.Interpreter;
using Robotersteuerung.Utils;
using Robotersteuerung.Models;
using AbcRobotCore;

namespace Robotersteuerung {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private RobotFieldWrapper? _fieldWrapper;
        private Program? _program;
        private RobotInterpreter? _interpreter;

        public MainWindow() {
            InitializeComponent();
        }

        private void LoadFieldButton_Click(object sender, RoutedEventArgs e) {
            string path = FieldPath.Text.Trim();
            if (string.IsNullOrEmpty(path)) {
                ErrorDisplay.Text = "Bitte geben Sie einen Pfad zur XML-Datei ein.";
                return;
            }

            try {
                // Die RobotField Custom Control parst das XML selbst
                // Daher geben wir den Pfad direkt an LoadField
                RobotFieldControl.LoadField(path);
                _fieldWrapper = new RobotFieldWrapper(RobotFieldControl);
                _interpreter = new RobotInterpreter(_fieldWrapper);
                
                ErrorDisplay.Text = "✓ Spielfeld geladen!";
                ExecuteButton.IsEnabled = false;
                ExecutionLog.Clear();
            } catch (Exception ex) {
                ErrorDisplay.Text = $"Fehler beim Laden: {ex.Message}\nZeile 2 = XML-Parse Fehler";
            }
        }

        private void LoadProgramButton_Click(object sender, RoutedEventArgs e) {
            string path = ProgramPath.Text.Trim();
            if (string.IsNullOrEmpty(path)) {
                ErrorDisplay.Text = "Bitte geben Sie einen Pfad zur Programm-Datei ein.";
                return;
            }

            try {
                string content = File.ReadAllText(path, Encoding.UTF8);
                ProgramText.Text = content;
                ErrorDisplay.Text = "";
            } catch (Exception ex) {
                ErrorDisplay.Text = $"Fehler beim Laden der Datei: {ex.Message}";
            }
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e) {
            if (_fieldWrapper == null) {
                ErrorDisplay.Text = "Bitte laden Sie zunächst ein Spielfeld.";
                return;
            }

            string programText = ProgramText.Text;
            if (string.IsNullOrEmpty(programText)) {
                ErrorDisplay.Text = "Bitte geben Sie ein Programm ein.";
                return;
            }

            try {
                Lexer lexer = new Lexer(programText);
                List<Token> tokens = lexer.Tokenize();

                Parser.Parser parser = new Parser.Parser(tokens);
                _program = parser.Parse();

                if (parser.Errors.Count > 0) {
                    StringBuilder sb = new StringBuilder();
                    foreach (var error in parser.Errors) {
                        sb.AppendLine(error);
                    }
                    ErrorDisplay.Text = sb.ToString();
                    ExecuteButton.IsEnabled = false;
                } else {
                    ErrorDisplay.Text = "✓ Programm erfolgreich analysiert!";
                    ExecuteButton.IsEnabled = true;
                }
            } catch (Exception ex) {
                ErrorDisplay.Text = $"Fehler beim Analysieren: {ex.Message}";
                ExecuteButton.IsEnabled = false;
            }
        }

        private async void ExecuteButton_Click(object sender, RoutedEventArgs e) {
            if (_program == null || _interpreter == null) {
                ErrorDisplay.Text = "Bitte analysieren Sie zunächst das Programm.";
                return;
            }

            ExecuteButton.IsEnabled = false;
            ParseButton.IsEnabled = false;
            ExecutionLog.Clear();
            CollectedLetters.Text = "(noch keine)";

            try {
                _interpreter.Execute(_program);

                foreach (var step in _interpreter.ExecutionHistory) {
                    ExecutionLog.AppendText($"{step}\n");
                    ExecutionLog.ScrollToEnd();
                    await Task.Delay(1000);
                }

                CollectedLetters.Text = _interpreter.GetCollectedLetters();
                ErrorDisplay.Text = "✓ Programm erfolgreich ausgeführt!";
            } catch (Exception ex) {
                ErrorDisplay.Text = $"Fehler bei der Ausführung: {ex.Message}";
            } finally {
                ExecuteButton.IsEnabled = true;
                ParseButton.IsEnabled = true;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            ExecutionLog.Clear();
            ErrorDisplay.Text = "";
            ProgramText.Clear();
            CollectedLetters.Text = "(noch keine)";
            ExecuteButton.IsEnabled = false;
        }
    }
}