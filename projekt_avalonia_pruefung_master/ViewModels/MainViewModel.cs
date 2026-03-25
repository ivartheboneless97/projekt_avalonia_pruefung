using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using projekt_avalonia_pruefung_master.Models;
using projekt_avalonia_pruefung_master.Data;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MessageBox.Avalonia.Enums;

namespace projekt_avalonia_pruefung_master.ViewModels;

/// <summary>
/// Das MainViewModel bildet die zentrale Steuereinheit der App (ViewModel im MVVM-Muster).
/// Es verknüpft die View (GUI) mit der Datenhaltung (TransactionManager).
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    // Zugriff auf die Datenhaltungs-Schicht (SQLite / EF Core)
    private readonly TransactionManager _manager;

    // --- Listen und Daten ---
    // Diese Liste ist an die UI gebunden und aktualisiert sich automatisch bei Änderungen
    [ObservableProperty]
    private ObservableCollection<Transaction> _transaktionen;

    // Werte für die Header-Cards in der GUI
    [ObservableProperty]
    private decimal _kontostand;

    [ObservableProperty]
    private decimal _verfuegbaresBudget;

    // --- Eingabefelder ---
    // Bindings für das Formular zur Erfassung neuer Buchungen
    [ObservableProperty]
    private string _neueBeschreibung = string.Empty;

    [ObservableProperty]
    private decimal _neuerBetrag;

    [ObservableProperty]
    private bool _istNeueAusgabe = true; // Bestimmt den Typ (Einnahme/Ausgabe) via ToggleSwitch

    [ObservableProperty]
    private bool _istFixkosten; // Markierung für wiederkehrende Kosten

    public MainViewModel()
    {
        _manager = new TransactionManager();
        _transaktionen = new ObservableCollection<Transaction>();

        // Initiales Laden der Daten aus der Datenbank beim Start der App
        LadeDaten();
    }

    /// <summary>
    /// Lädt alle Transaktionen über den Manager aus der SQLite-Datenbank.
    /// </summary>
    [RelayCommand]
    public void LadeDaten()
    {
        // Abruf der sortierten Daten aus der Persistenzschicht
        var daten = _manager.LadeAlleTransaktionen();

        Transaktionen.Clear();
        foreach (var t in daten)
        {
            Transaktionen.Add(t);
        }

        // Berechnet die Summen für das Dashboard neu
        BerechneBilanz();
    }

    /// <summary>
    /// Führt die Echtzeit-Berechnung der Salden basierend auf der aktuellen Liste durch.
    /// </summary>
    private void BerechneBilanz()
    {
        // 1. Gesamter Kontostand: Summe aller Einnahmen minus Summe aller Ausgaben
        decimal einnahmen = Transaktionen.Where(t => !t.IstAusgabe).Sum(t => t.Betrag);
        decimal ausgaben = Transaktionen.Where(t => t.IstAusgabe).Sum(t => t.Betrag);
        Kontostand = einnahmen - ausgaben;

        // 2. Verfügbares Budget: Aktueller Stand unter Berücksichtigung der Fixkosten
        decimal fixkosten = Transaktionen.Where(t => t.IstAusgabe && t.IstFixkosten).Sum(t => t.Betrag);
        VerfuegbaresBudget = Kontostand;
    }

    /// <summary>
    /// Erstellt eine neue Transaktion und speichert diese dauerhaft in der Datenbank.
    /// </summary>
    [RelayCommand]
    public void Speichern()
    {
        // Validierung der Eingabe (Beschreibung darf nicht leer sein, Betrag > 0)
        if (string.IsNullOrWhiteSpace(NeueBeschreibung) || NeuerBetrag <= 0)
            return;

        // Erstellung des Daten-Modells aus den Formular-Eingaben
        var neueTransaktion = new Transaction
        {
            Beschreibung = NeueBeschreibung,
            Betrag = NeuerBetrag,
            IstAusgabe = IstNeueAusgabe,
            IstFixkosten = IstFixkosten,
            Datum = DateTime.Now,
            Kategorie = IstFixkosten ? "Fixkosten" : "Variabel" // Automatische Kategorisierung
        };

        // Übergabe an die Persistenzschicht
        _manager.SpeichereTransaktion(neueTransaktion);

        // UI-Felder nach erfolgreichem Speichern leeren (Reset)
        NeueBeschreibung = string.Empty;
        NeuerBetrag = 0;
        IstFixkosten = false;

        // Liste und Berechnungen aktualisieren
        LadeDaten();
    }

    /// <summary>
    /// Entfernt eine Transaktion nach Bestätigung durch den Nutzer.
    /// </summary>
    [RelayCommand]
    public async Task Loeschen(Transaction t)
    {
        if (t == null) return;

        // Sicherheitsabfrage via MessageBox (asynchron)
        var box = MessageBoxManager.GetMessageBoxStandard(
            "Löschen bestätigen",
            "Möchten Sie diesen Eintrag wirklich löschen?",
            ButtonEnum.YesNo,
            Icon.Question);

        var result = await box.ShowAsync();

        // Nur wenn der Nutzer explizit mit "Ja" bestätigt
        if (result == ButtonResult.Yes)
        {
            // Löschen in der Datenbank via ID
            _manager.LoescheTransaktion(t.Id);

            // Ansicht aktualisieren
            LadeDaten();
        }
    }
}