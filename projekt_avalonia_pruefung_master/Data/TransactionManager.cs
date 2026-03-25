using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using projekt_avalonia_pruefung_master.Models; // Wichtig für die Transaction-Klasse
using System.Collections.Generic;
using System.Linq;

namespace projekt_avalonia_pruefung_master.Data
{
    /// <summary>
    /// Der TransactionManager bildet die Logikschicht für den Datenbankzugriff.
    /// Er stellt Methoden bereit, um Daten zu speichern, zu laden und zu löschen.
    /// </summary>
    public class TransactionManager
    {
        private readonly DbContextOptions<DataContext> _options;

        public TransactionManager()
        {
            // Konfiguration der Datenbankverbindung für EF Core
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlite("Data Source=meineDatenbank.db");
            _options = optionsBuilder.Options;

            // Stellt beim Start der App sicher, dass die Datenbankdatei und die Tabellen existieren
            using (var context = new DataContext(_options))
            {
                context.Database.EnsureCreated();
            }
        }

        /// <summary>
        /// Fügt eine neue Transaktion der Datenbank hinzu.
        /// </summary>
        public void SpeichereTransaktion(Transaction t)
        {
            using (var context = new DataContext(_options))
            {
                // Das Objekt wird zur Tabelle hinzugefügt und die Änderungen festgeschrieben
                context.Transaktionen.Add(t);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Ruft alle gespeicherten Transaktionen ab.
        /// </summary>
        /// <returns>Eine nach Datum absteigend sortierte Liste der Transaktionen.</returns>
        public List<Transaction> LadeAlleTransaktionen()
        {
            using (var context = new DataContext(_options))
            {
                // Die Daten werden nach Datum sortiert geladen, damit die neuesten oben stehen
                return context.Transaktionen.OrderByDescending(t => t.Datum).ToList();
            }
        }

        /// <summary>
        /// Entfernt eine spezifische Transaktion anhand ihrer eindeutigen ID.
        /// </summary>
        public void LoescheTransaktion(int id)
        {
            using (var context = new DataContext(_options))
            {
                // 1. Suche nach dem Datensatz in der Datenbank
                var transaktion = context.Transaktionen.FirstOrDefault(t => t.Id == id);

                if (transaktion != null)
                {
                    // 2. Markieren des Datensatzes als gelöscht
                    context.Transaktionen.Remove(transaktion);

                    // 3. Synchronisation mit der SQLite-Datei
                    context.SaveChanges();
                }
            }
        }
    }
}