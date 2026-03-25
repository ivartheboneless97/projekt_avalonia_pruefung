using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using projekt_avalonia_pruefung_master.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace projekt_avalonia_pruefung_master.Data
{
    /// <summary>
    /// Der DataContext ist die Schnittstelle zur Datenbank (Entity Framework Core).
    /// Er verwaltet die Verbindung zur SQLite-Datenbank und bildet die Tabellen ab.
    /// </summary>
    public class DataContext : DbContext
    {
        // Repräsentiert die Tabelle "Transaktionen" in der Datenbank basierend auf dem Model "Transaction"
        public DbSet<Transaction> Transaktionen { get; set; }

        // Konstruktor für die Initialisierung des Kontextes mit Optionen
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// Konfiguriert die Datenbankverbindung. 
        /// Hier wird festgelegt, dass SQLite genutzt wird und wo die Datei liegt.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Erstellt den absoluten Pfad zur Datenbankdatei im Anwendungsverzeichnis
            string dbPath = Path.Combine(
                AppContext.BaseDirectory,
                "meineDatenbank.db"
            );

            // Konfiguration von SQLite mit dem ermittelten Pfad
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            // Debug-Ausgabe zur Kontrolle des Speicherortes während der Entwicklung
            System.Diagnostics.Debug.WriteLine($"[DB PATH CHECK] Datenbank wird erwartet unter: {dbPath}");

            // Definiert die SQLite-Datenquelle (erstellt die Datei automatisch im Ausführungsverzeichnis)
            optionsBuilder.UseSqlite("Data Source=meineDatenbank.db");
        }
    }
}