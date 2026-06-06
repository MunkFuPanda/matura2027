using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;            // OpenFileDialog
using MosaikWerkstatt.Language;
using MosaikWerkstatt.Persistence;

namespace MosaikWerkstatt
{
    // -------------------------------------------------------------------------
    // CODE-BEHIND = die "Verdrahtung": Datei laden -> tokenize -> parse ->
    // interpret -> Frames getaktet zeichnen. Die eigentliche Sprach-Logik
    // liegt komplett in Language\ und wird hier nur benutzt.
    //
    // Bei einem Vorgabe-Projekt: NUR diese Verdrahtung in das vorgegebene
    // Code-Behind uebernehmen, die Language\-Dateien als eigene .cs dazulegen.
    // -------------------------------------------------------------------------
    public partial class MainWindow : Window
    {
        private const int Rows = 9;
        private const int Cols = 9;

        private Border[,] _cellViews;       // die sichtbaren Zellen
        private readonly ProgramStore _store = new ProgramStore();

        public MainWindow()
        {
            InitializeComponent();
            BuildField();
            RefreshSavedList();
        }

        // --- Feld einmalig aufbauen (UniformGrid mit Rows*Cols Zellen) ------
        private void BuildField()
        {
            FieldHost.Rows = Rows;
            FieldHost.Columns = Cols;
            _cellViews = new Border[Rows, Cols];

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    var cell = new Border
                    {
                        BorderBrush = Brushes.LightGray,
                        BorderThickness = new Thickness(0.5),
                        Background = Brushes.White
                    };
                    _cellViews[r, c] = cell;
                    FieldHost.Children.Add(cell);
                }
            }
        }

        // =====================================================================
        // BUTTON-HANDLER
        // =====================================================================

        // Datei laden -> Text in den Editor.
        private void OnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Mosaik-Programme (*.mosaik;*.txt)|*.mosaik;*.txt|Alle Dateien (*.*)|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                CodeBox.Text = File.ReadAllText(dlg.FileName);
                SetStatus("Geladen: " + dlg.FileName, false);
            }
        }

        // Nur Syntax pruefen (parsen ohne ausfuehren).
        private void OnParse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var interp = new MosaikInterpreter(Rows, Cols);
                interp.Parse(CodeBox.Text);
                SetStatus("OK - Syntax korrekt.", false);
            }
            catch (ParseException ex)
            {
                SetStatus("SYNTAXFEHLER: " + ex.Message, true);
            }
        }

        // Parsen + ausfuehren + getaktet animieren.
        private async void OnRun_Click(object sender, RoutedEventArgs e)
        {
            ClearField();
            List<Frame> frames;
            try
            {
                var interp = new MosaikInterpreter(Rows, Cols);
                frames = interp.Run(CodeBox.Text);   // parse + interpret
            }
            catch (ParseException ex)
            {
                SetStatus("SYNTAXFEHLER: " + ex.Message, true);
                return;
            }
            catch (RuntimeException ex)
            {
                SetStatus("LAUFZEITFEHLER: " + ex.Message, true);
                return;
            }

            SetStatus("Laeuft... (" + frames.Count + " Schritte)", false);

            // Frames getaktet abspielen: 1 Sekunde Pause zwischen Schritten.
            // await Task.Delay haelt die GUI responsiv (kein Einfrieren).
            foreach (var frame in frames)
            {
                RenderFrame(frame);
                await Task.Delay(1000);
            }
            SetStatus("Fertig.", false);
        }

        // --- Persistenz (EF Core / SQLite) ---------------------------------
        private void OnSaveDb_Click(object sender, RoutedEventArgs e)
        {
            // Eigener kleiner Dialog (siehe InputDialog.xaml) statt
            // Fremd-Abhaengigkeiten -> zeigt zugleich, wie man Dialoge baut.
            string name = InputDialog.Ask(this, "Speichern", "Name fuer das Programm:", "Mein Programm");
            if (string.IsNullOrWhiteSpace(name)) return;

            _store.Save(name, CodeBox.Text);
            RefreshSavedList();
            SetStatus("In Datenbank gespeichert: " + name, false);
        }

        private void OnLoadDb_Click(object sender, RoutedEventArgs e)
        {
            RefreshSavedList();
            SetStatus("Liste aktualisiert.", false);
        }

        private void OnSavedList_DoubleClick(object sender, RoutedEventArgs e)
        {
            if (SavedList.SelectedItem is SavedProgram p)
            {
                CodeBox.Text = p.Source;
                SetStatus("Geladen aus DB: " + p.Name, false);
            }
        }

        private void OnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // =====================================================================
        // HILFSFUNKTIONEN
        // =====================================================================
        private void RefreshSavedList()
        {
            SavedList.ItemsSource = null;
            SavedList.ItemsSource = _store.GetAll();
            SavedList.DisplayMemberPath = "Name";
        }

        private void ClearField()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                {
                    _cellViews[r, c].Background = Brushes.White;
                    _cellViews[r, c].BorderBrush = Brushes.LightGray;
                    _cellViews[r, c].BorderThickness = new Thickness(0.5);
                }
        }

        private void RenderFrame(Frame frame)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                {
                    string color = frame.Cells[r, c];
                    _cellViews[r, c].Background =
                        color == null ? Brushes.White : NameToBrush(color);
                    _cellViews[r, c].BorderBrush = Brushes.LightGray;
                    _cellViews[r, c].BorderThickness = new Thickness(0.5);
                }

            // Cursor hervorheben.
            var cur = _cellViews[frame.CursorRow, frame.CursorCol];
            cur.BorderBrush = Brushes.DodgerBlue;
            cur.BorderThickness = new Thickness(2);
        }

        private static Brush NameToBrush(string name)
        {
            switch (name)
            {
                case "Red": return Brushes.Red;
                case "Green": return Brushes.Green;
                case "Blue": return Brushes.Blue;
                case "Yellow": return Brushes.Gold;
                case "White": return Brushes.White;
                case "Black": return Brushes.Black;
                default: return Brushes.Gray;
            }
        }

        private void SetStatus(string message, bool isError)
        {
            StatusText.Text = message;
            StatusText.Foreground = isError ? Brushes.Red : Brushes.Black;
        }
    }
}
