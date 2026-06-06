using System;

namespace MosaikWerkstatt.Persistence
{
    // ENTITY: eine Zeile in der Tabelle "SavedPrograms".
    // EF Core erkennt die Property "Id" automatisch als Primaerschluessel
    // (Autoincrement).
    public class SavedProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }      // der Programmtext
        public DateTime CreatedAt { get; set; }
    }
}
