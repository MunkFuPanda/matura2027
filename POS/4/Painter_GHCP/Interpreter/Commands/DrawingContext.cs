using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Painter.Interpreter.Commands
{
    /// <summary>
    /// Der DrawingContext repräsentiert den aktuellen Zustand des Zeichnens.
    /// Er speichert:
    /// - Die aktuelle Position (X, Y)
    /// - Die aktuelle Ausrichtung (Winkel)
    /// - Die aktuelle Zeichenfarbe
    /// - Die Linien, die bereits gezeichnet wurden
    /// </summary>
    public class DrawingContext
    {
        // Aktuelle X-Koordinate des Stiftes
        public double CurrentX { get; set; }

        // Aktuelle Y-Koordinate des Stiftes
        public double CurrentY { get; set; }

        // Aktueller Winkel in Grad (0° = rechts, 90° = unten, etc.)
        public int CurrentAngle { get; set; }

        // Aktuelle Zeichenfarbe
        public Color CurrentColor { get; set; }

        // Liste aller Linien, die gezeichnet wurden
        public List<LineSegment> Lines { get; private set; }

        public DrawingContext()
        {
            // Starte in der Mitte (wird später kalibriert)
            CurrentX = 0;
            CurrentY = 0;

            // Starte mit nach rechts gerichteter Orientierung
            CurrentAngle = 0;

            // Standardfarbe ist schwarz
            CurrentColor = Colors.Black;

            // Initialisiere die Liste der Linien
            Lines = new List<LineSegment>();
        }

        /// <summary>
        /// Fügt eine neue Linie zur Zeichnung hinzu.
        /// </summary>
        public void AddLine(double x1, double y1, double x2, double y2, Color color)
        {
            Lines.Add(new LineSegment
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Color = color
            });
        }

        /// <summary>
        /// Setzt den Kontext auf die Anfangswerte zurück.
        /// </summary>
        public void Reset()
        {
            CurrentX = 0;
            CurrentY = 0;
            CurrentAngle = 0;
            CurrentColor = Colors.Black;
            Lines.Clear();
        }
    }

    /// <summary>
    /// Repräsentiert eine gezeichnete Linie mit ihren Eigenschaften.
    /// </summary>
    public class LineSegment
    {
        // Startkoordinaten
        public double X1 { get; set; }
        public double Y1 { get; set; }

        // Endkoordinaten
        public double X2 { get; set; }
        public double Y2 { get; set; }

        // Farbe der Linie
        public Color Color { get; set; }
    }
}
