using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MosaikWerkstatt.Persistence
{
    // ---------------------------------------------------------------------
    // REPOSITORY: kapselt alle DB-Zugriffe. Die GUI ruft nur Save/GetAll/...
    // auf und sieht von EF Core nichts. Zeigt CRUD + LINQ.
    // ---------------------------------------------------------------------
    public class ProgramStore
    {
        public ProgramStore()
        {
            // Legt die DB + Tabellen an, falls noch nicht vorhanden.
            // (Fuer eine kleine Angabe einfacher als Migrations.)
            using (var db = new AppDbContext())
                db.Database.EnsureCreated();
        }

        // CREATE
        public void Save(string name, string source)
        {
            using (var db = new AppDbContext())
            {
                db.SavedPrograms.Add(new SavedProgram
                {
                    Name = name,
                    Source = source,
                    CreatedAt = DateTime.Now
                });
                db.SaveChanges();   // schreibt in die DB
            }
        }

        // READ (LINQ: sortiert nach Datum, neueste zuerst)
        public List<SavedProgram> GetAll()
        {
            using (var db = new AppDbContext())
            {
                return db.SavedPrograms
                         .OrderByDescending(p => p.CreatedAt)
                         .ToList();
            }
        }

        // READ mit Filter (LINQ Where) -- Beispiel fuer eine Suche
        public List<SavedProgram> Search(string term)
        {
            using (var db = new AppDbContext())
            {
                return db.SavedPrograms
                         .Where(p => p.Name.Contains(term))
                         .OrderBy(p => p.Name)
                         .ToList();
            }
        }

        // DELETE
        public void Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var entity = db.SavedPrograms.FirstOrDefault(p => p.Id == id);
                if (entity != null)
                {
                    db.SavedPrograms.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
