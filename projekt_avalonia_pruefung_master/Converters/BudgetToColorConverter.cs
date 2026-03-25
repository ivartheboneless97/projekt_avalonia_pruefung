using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace projekt_avalonia_pruefung_master.Converters;

/// <summary>
/// Dieser Konverter visualisiert den finanziellen Status anhand des Betrags.
/// Er wird für die Anzeige des Gesamtsaldos und des verfügbaren Budgets genutzt.
/// </summary>
public class BudgetToColorConverter : IValueConverter
{
    // Wandelt den numerischen Budget-Wert in eine Signalfarbe um
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Prüfung, ob der Wert ein gültiger Dezimalwert (Betrag) ist
        if (value is decimal budget)
        {
            // Logik für die farbliche Statusanzeige:
            // Ein negativer Kontostand (< 0) wird Crimson (Rot) markiert,
            // ein positiver Saldo wird MediumSeaGreen (Grün) dargestellt.
            return budget < 0 ? Brushes.Crimson : Brushes.MediumSeaGreen;
        }

        // Fallback-Farbe (Grau), falls kein gültiger Betrag vorliegt
        return Brushes.Gray;
    }

    // Eine Rückumwandlung von Farbe zu Zahl ist für die Anzeige nicht erforderlich
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}