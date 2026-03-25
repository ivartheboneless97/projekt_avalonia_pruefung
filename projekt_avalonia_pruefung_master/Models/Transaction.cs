using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt_avalonia_pruefung_master.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Betrag { get; set; }
        public DateTime Datum { get; set; }
        public string Beschreibung { get; set; }
        public string Kategorie {  get; set; }
        public bool IstAusgabe {  get; set; }
        public bool IstFixkosten { get; set; } // Neu: Damit wir Miete von Pizza unterscheiden können
    }
}
