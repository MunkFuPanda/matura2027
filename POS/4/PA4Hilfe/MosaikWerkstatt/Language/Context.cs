using System;
using System.Collections.Generic;

namespace MosaikWerkstatt.Language
{
    // ---------------------------------------------------------------------
    // CONTEXT  (im Interpreter-Pattern der "Context", den jede
    // Interpret(...)-Methode bekommt und veraendert).
    //
    // Haelt den kompletten Laufzeit-Zustand:
    //  - Gittergroesse
    //  - Cursor-Position
    //  - aktuelle Farbe
    //  - die gemalten Zellen (Farbe je Zelle, null = leer)
    //
    // ZUSAETZLICH zeichnet er bei jeder sichtbaren Aenderung einen "Frame"
    // auf (Schnappschuss). Die GUI spielt diese Frames danach getaktet ab.
    // -> Dadurch bleibt diese Klasse komplett GUI-FREI (kein WPF).
    //    Du kannst sie 1:1 in jedes Vorgabe-Projekt werfen.
    //
    // Tradeoff (bewusst, fuer die PA ok): Es werden volle Schnappschuesse
    // gespeichert. Bei sehr grossen Gittern/sehr vielen Schritten waere das
    // speicherintensiv -> dann lieber Deltas speichern. Fuer die Angabe egal.
    // ---------------------------------------------------------------------

    // Ein Schnappschuss des Felds zu einem Zeitpunkt (fuer die Animation).
    public class Frame
    {
        public int CursorRow { get; set; }
        public int CursorCol { get; set; }
        public string[,] Cells { get; set; } // Kopie der Farben
    }

    public class Context
    {
        public int Rows { get; }
        public int Cols { get; }
        public int CursorRow { get; private set; }
        public int CursorCol { get; private set; }
        public string CurrentColor { get; private set; }

        private readonly string[,] _cells; // logischer Zustand
        public List<Frame> Frames { get; } = new List<Frame>();

        public Context(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            _cells = new string[rows, cols];
            CursorRow = 0;
            CursorCol = 0;
            CurrentColor = "Black";
        }

        // Aktuelle Zelle mit aktueller Farbe einfaerben + Frame aufzeichnen.
        public void PaintCurrent()
        {
            _cells[CursorRow, CursorCol] = CurrentColor;
            Snapshot();
        }

        public void SetColor(string color)
        {
            CurrentColor = color;
            PaintCurrent(); // Farbwechsel faerbt auch die aktuelle Zelle
        }

        // Einen Schritt bewegen, dabei die Zielzelle faerben.
        // Verlaesst der Cursor das Feld -> RuntimeException.
        public void Move(string direction)
        {
            int nr = CursorRow;
            int nc = CursorCol;
            switch (direction)
            {
                case "UP": nr--; break;
                case "DOWN": nr++; break;
                case "LEFT": nc--; break;
                case "RIGHT": nc++; break;
                default: throw new RuntimeException("Unbekannte Richtung: " + direction);
            }
            if (nr < 0 || nr >= Rows || nc < 0 || nc >= Cols)
                throw new RuntimeException(
                    "Cursor verlaesst das Feld (Richtung " + direction + ").");

            CursorRow = nr;
            CursorCol = nc;
            PaintCurrent();
        }

        // Bedingung: Kann man einen Schritt in die Richtung gehen,
        // ohne das Feld zu verlassen?  (fuer WHILE / IF)
        public bool CanMove(string direction)
        {
            switch (direction)
            {
                case "UP": return CursorRow > 0;
                case "DOWN": return CursorRow < Rows - 1;
                case "LEFT": return CursorCol > 0;
                case "RIGHT": return CursorCol < Cols - 1;
                default: throw new RuntimeException("Unbekannte Richtung: " + direction);
            }
        }

        private void Snapshot()
        {
            var copy = new string[Rows, Cols];
            Array.Copy(_cells, copy, _cells.Length);
            Frames.Add(new Frame
            {
                CursorRow = CursorRow,
                CursorCol = CursorCol,
                Cells = copy
            });
        }
    }
}
