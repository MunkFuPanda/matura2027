using System;
using System.Windows;
using Painter.Interpreter;

namespace Painter
{
    /// <summary>
    /// Die MainWindow-Klasse verwaltet die Benutzeroberfläche.
    /// Sie ist zuständig für:
    /// - Entgegennahme des Quellcodes vom Benutzer
    /// - Aufrufen des Interpreters
    /// - Anzeige der Fehler oder der Visualisierung
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProgramInterpreter interpreter;

        public MainWindow()
        {
            InitializeComponent();
            interpreter = new ProgramInterpreter();
        }

        /// <summary>
        /// Event-Handler für den "Ausführen"-Button.
        /// Liest den Code aus der Textbox und führt ihn aus.
        /// </summary>
        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            // Leere die Fehlerbox
            ErrorTextBox.Text = "";

            // Hole den Code aus der Textbox
            string sourceCode = CodeTextBox.Text;

            if (string.IsNullOrWhiteSpace(sourceCode))
            {
                ErrorTextBox.Text = "Bitte geben Sie Quellcode ein.";
                return;
            }

            try
            {
                // Führe den Code aus
                var drawingContext = interpreter.Execute(sourceCode);

                // Zeige die Visualisierung
                PainterControl.SetDrawingContext(drawingContext);

                // Zeige Erfolg
                if (drawingContext.Lines.Count > 0)
                    ErrorTextBox.Text = $"Erfolgreich! {drawingContext.Lines.Count} Linien gezeichnet.";
                else
                    ErrorTextBox.Text = "Erfolgreich! Keine Linien gezeichnet.";
            }
            catch (ParseException ex)
            {
                // Zeige Fehler mit Zeilennummer
                ErrorTextBox.Text = ex.ToString();
                PainterControl.Clear();
            }
            catch (Exception ex)
            {
                // Unerwarteter Fehler
                ErrorTextBox.Text = $"Unerwarteter Fehler: {ex.Message}";
                PainterControl.Clear();
            }
        }

        /// <summary>
        /// Event-Handler für den "Löschen"-Button.
        /// Löscht die Zeichnung und die Fehlerbox.
        /// </summary>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBox.Text = "";
            PainterControl.Clear();
        }
    }
}