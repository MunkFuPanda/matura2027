using System;
using System.Windows.Media;

namespace Painter.Interpreter.Commands
{
    /// <summary>
    /// Das Command Pattern (Teil des Interpreter Patterns):
    /// Jede Anweisung (TURN, DRAW, COLOR, etc.) wird als separates Command-Objekt repräsentiert.
    /// Dies ermöglicht einfache Verwaltung, Ausführung und Fehlerbehandlung.
    /// </summary>

    /// <summary>
    /// Basis-Interface für alle Befehle.
    /// Alle Befehle müssen diese Schnittstelle implementieren.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Führt den Befehl mit dem gegebenen Kontext aus.
        /// </summary>
        /// <param name="context">Der Zustand des Zeichnens (Position, Winkel, Farbe, etc.)</param>
        void Execute(DrawingContext context);
    }

    /// <summary>
    /// Befehl zum Drehen des Stiftes um einen bestimmten Winkel.
    /// </summary>
    public class TurnCommand : ICommand
    {
        // Drehrichtung: links oder rechts
        private readonly TurnDirection direction;

        // Drehwinkel in Grad
        private readonly int angle;

        public enum TurnDirection
        {
            LEFT,
            RIGHT
        }

        public TurnCommand(TurnDirection direction, int angle)
        {
            this.direction = direction;
            this.angle = angle;
        }

        public void Execute(DrawingContext context)
        {
            // Wenn wir nach rechts drehen, addieren wir den Winkel
            // Wenn wir nach links drehen, subtrahieren wir den Winkel
            int rotationAmount = direction == TurnDirection.RIGHT ? angle : -angle;
            context.CurrentAngle = (context.CurrentAngle + rotationAmount) % 360;

            // Stelle sicher, dass der Winkel im Bereich [0, 360) liegt
            if (context.CurrentAngle < 0)
                context.CurrentAngle += 360;
        }

        public override string ToString() => $"TURN {direction} {angle}";
    }

    /// <summary>
    /// Befehl zum Zeichnen einer Linie mit angegebener Länge.
    /// </summary>
    public class DrawCommand : ICommand
    {
        // Länge der zu zeichnenden Linie
        private readonly int length;

        public DrawCommand(int length)
        {
            this.length = length;
        }

        public void Execute(DrawingContext context)
        {
            // Berechne die neue Endposition der Linie basierend auf Winkel und Länge
            // Der Winkel 0° zeigt nach rechts (Osten)
            // Der Winkel 90° zeigt nach unten (Süden)

            double angleInRadians = context.CurrentAngle * Math.PI / 180.0;
            double newX = context.CurrentX + length * Math.Cos(angleInRadians);
            double newY = context.CurrentY + length * Math.Sin(angleInRadians);

            // Füge eine Linie zur Zeichnung hinzu
            context.AddLine(context.CurrentX, context.CurrentY, newX, newY, context.CurrentColor);

            // Aktualisiere die aktuelle Position
            context.CurrentX = newX;
            context.CurrentY = newY;
        }

        public override string ToString() => $"DRAW {length}";
    }

    /// <summary>
    /// Befehl zum Ändern der Zeichenfarbe.
    /// </summary>
    public class ColorCommand : ICommand
    {
        // Die neue Farbe
        private readonly Color color;

        public ColorCommand(Color color)
        {
            this.color = color;
        }

        public void Execute(DrawingContext context)
        {
            context.CurrentColor = color;
        }

        public override string ToString() => $"COLOR {color}";
    }

    /// <summary>
    /// Befehl zum Wiederholen einer Liste von Befehlen.
    /// Implementiert die Schleife (FOR-Schleife).
    /// </summary>
    public class RepeatCommand : ICommand
    {
        // Anzahl der Wiederholungen
        private readonly int count;

        // Die Befehle, die wiederholt werden sollen
        private readonly ICommand[] commands;

        public RepeatCommand(int count, ICommand[] commands)
        {
            this.count = count;
            this.commands = commands;
        }

        public void Execute(DrawingContext context)
        {
            // Führe alle Befehle 'count' Mal aus
            for (int i = 0; i < count; i++)
            {
                foreach (var command in commands)
                {
                    command.Execute(context);
                }
            }
        }

        public override string ToString() => $"FOR {count} {{ ... }}";
    }

    /// <summary>
    /// Befehl für einen Block von mehreren Befehlen.
    /// Diese werden einfach hintereinander ausgeführt.
    /// </summary>
    public class BlockCommand : ICommand
    {
        // Die Befehle im Block
        private readonly ICommand[] commands;

        public BlockCommand(ICommand[] commands)
        {
            this.commands = commands;
        }

        public void Execute(DrawingContext context)
        {
            foreach (var command in commands)
            {
                command.Execute(context);
            }
        }

        public override string ToString() => "{ ... }";
    }
}
