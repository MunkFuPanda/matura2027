using System;

namespace MosaikWerkstatt.Language
{
    // ---------------------------------------------------------------------
    // Eigene Exception-Typen. Wichtig fuer die PA:
    //  - ParseException  -> Syntax-/Tokenfehler BEIM PARSEN (mit Zeile/Spalte)
    //  - RuntimeException -> Fehler WAEHREND der Ausfuehrung (z.B. Feld verlassen)
    //
    // Eigene Typen statt nur "Exception", damit du in der GUI gezielt
    // fangen und unterschiedlich anzeigen kannst.
    // ---------------------------------------------------------------------

    public class ParseException : Exception
    {
        public int Line { get; }
        public int Column { get; }

        public ParseException(string message, int line, int column)
            : base("Zeile " + line + ", Spalte " + column + ": " + message)
        {
            Line = line;
            Column = column;
        }
    }

    public class RuntimeException : Exception
    {
        public RuntimeException(string message) : base(message) { }
    }
}
