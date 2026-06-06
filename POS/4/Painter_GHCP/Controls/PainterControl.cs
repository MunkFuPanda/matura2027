using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Painter.Interpreter.Commands;

namespace Painter.Controls
{
    /// <summary>
    /// Das PainterControl ist ein benutzerdefiniertes WPF-Control,
    /// das den zeichnerischen Kontext visualisiert.
    /// </summary>
    public class PainterControl : Canvas
    {
        // Die Linien, die gezeichnet werden sollen (von unserem Interpreter)
        private Interpreter.Commands.DrawingContext painterDrawingContext;

        // Zoom-Level für die Anzeige
        private double zoomLevel = 1.0;

        // Offset für die Verschiebung der Zeichnung
        private double offsetX = 0;
        private double offsetY = 0;

        // Flag ob Viewport berechnet wurde
        private bool viewportCalculated = false;

        public PainterControl()
        {
            // Setze weiße Hintergrundfarbe
            Background = Brushes.White;

            // Höre auf Größenänderungen
            SizeChanged += (s, e) => 
            {
                // Setze das Flag zurück, um die Berechnung des Viewports bei der nächsten Neuzeichnung zu erzwingen
                viewportCalculated = false;
                InvalidateVisual();
            };
        }

        /// <summary>
        /// Setzt den Zeichnungskontext und zeichnet neu.
        /// </summary>
        public void SetDrawingContext(Interpreter.Commands.DrawingContext context)
        {
            this.painterDrawingContext = context;
            viewportCalculated = false;  // Erzwinge Neuberechnung des Viewports

            // Veranlasse ein Neuzeichnen
            InvalidateVisual();
        }

        /// <summary>
        /// Löscht die Zeichnung.
        /// </summary>
        public void Clear()
        {
            painterDrawingContext = null;
            viewportCalculated = false;
            InvalidateVisual();
        }

        /// <summary>
        /// Diese Methode wird von WPF aufgerufen, wenn das Control neu gezeichnet werden soll.
        /// Hier zeichnen wir alle Linien.
        /// </summary>
        protected override void OnRender(System.Windows.Media.DrawingContext wpfDrawingContext)
        {
            base.OnRender(wpfDrawingContext);

            // Zeichne den weißen Hintergrund
            wpfDrawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, ActualWidth, ActualHeight));

            // Wenn kein Kontext vorhanden ist, nichts zeichnen
            if (this.painterDrawingContext == null || this.painterDrawingContext.Lines.Count == 0)
                return;

            // Berechne Viewport, wenn noch nicht geschehen und die Größe gültig ist
            if (!viewportCalculated && ActualWidth > 0 && ActualHeight > 0)
            {
                CalculateViewport();
                viewportCalculated = true;
            }

            // Zeichne jede Linie
            foreach (var line in this.painterDrawingContext.Lines)
            {
                DrawLine(wpfDrawingContext, line);
            }
        }

        /// <summary>
        /// Zeichnet eine einzelne Linie mit Berücksichtigung von Zoom und Offset.
        /// </summary>
        private void DrawLine(System.Windows.Media.DrawingContext wpfDrawingContext, Interpreter.Commands.LineSegment line)
        {
            // Transformiere die Koordinaten mit Zoom und Offset
            double x1 = (line.X1 * zoomLevel) + offsetX;
            double y1 = (line.Y1 * zoomLevel) + offsetY;
            double x2 = (line.X2 * zoomLevel) + offsetX;
            double y2 = (line.Y2 * zoomLevel) + offsetY;

            // Erstelle einen Stift mit der Farbe der Linie
            var pen = new Pen(new SolidColorBrush(line.Color), 2);
            pen.EndLineCap = PenLineCap.Round;
            pen.StartLineCap = PenLineCap.Round;

            // Zeichne die Linie
            wpfDrawingContext.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));
        }

        /// <summary>
        /// Berechnet automatisch Zoom und Offset, um alle Linien optimal anzuzeigen.
        /// </summary>
        private void CalculateViewport()
        {
            if (painterDrawingContext == null || painterDrawingContext.Lines.Count == 0)
                return;

            // Finde die Grenzen aller Linien
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            foreach (var line in painterDrawingContext.Lines)
            {
                minX = Math.Min(minX, Math.Min(line.X1, line.X2));
                maxX = Math.Max(maxX, Math.Max(line.X1, line.X2));
                minY = Math.Min(minY, Math.Min(line.Y1, line.Y2));
                maxY = Math.Max(maxY, Math.Max(line.Y1, line.Y2));
            }

            // Berechne die Größe der Zeichnung
            double width = maxX - minX;
            double height = maxY - minY;

            // Verhindere Division durch Null
            if (width < 1) width = 1;
            if (height < 1) height = 1;

            // Berechne Zoom, um die Zeichnung in das Control zu passen
            // Mit 80% Padding (etwas mehr Raum)
            double availableWidth = ActualWidth * 0.8;
            double availableHeight = ActualHeight * 0.8;

            zoomLevel = Math.Min(availableWidth / width, availableHeight / height);
            
            // Stelle sicher dass wir nicht zu weit rauszoomen
            zoomLevel = Math.Max(zoomLevel, 0.5);

            // Berechne Offset, um die Zeichnung zu zentrieren
            double scaledWidth = width * zoomLevel;
            double scaledHeight = height * zoomLevel;

            offsetX = (ActualWidth - scaledWidth) / 2 - (minX * zoomLevel);
            offsetY = (ActualHeight - scaledHeight) / 2 - (minY * zoomLevel);
        }
    }
}
