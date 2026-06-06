using Microsoft.EntityFrameworkCore;

namespace MosaikWerkstatt.Persistence
{
    // ---------------------------------------------------------------------
    // DbContext = die Bruecke zwischen C#-Objekten und der SQLite-Datei.
    // Eine DbSet<T>-Property pro Tabelle.
    //
    // Waldwunder-Stil: UseSqlite mit einer lokalen .db-Datei.
    // ---------------------------------------------------------------------
    public class AppDbContext : DbContext
    {
        public DbSet<SavedProgram> SavedPrograms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Datei wird im Programmverzeichnis angelegt, falls nicht vorhanden.
            options.UseSqlite("Data Source=mosaik.db");
        }
    }
}
