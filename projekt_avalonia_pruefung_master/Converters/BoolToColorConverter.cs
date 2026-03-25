using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace projekt_avalonia_pruefung_master.Converters;

/// <summary>
/// Dieser Konverter hilft dabei, logische Zustände (bool) in Farben (Brushes) umzuwandeln.
/// Er wird im XAML genutzt, um Beträge je nach Typ optisch hervorzuheben.
/// </summary>
public class BoolToColorConverter : IValueConverter
{
    // Die Convert-Methode steuert die visuelle Logik beim Laden der Daten
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Prüft, ob der übergebene Wert ein bool ist (bezogen auf IstAusgabe)
        if (value is bool istAusgabe)
        {
            // Wenn es eine Ausgabe ist, wird Crimson (Rot) zurückgegeben,
            // ansonsten MediumSeaGreen (Grün) für Einnahmen.
            return istAusgabe ? Brushes.Crimson : Brushes.MediumSeaGreen;
        }

        // Standardfarbe, falls der Wert nicht bestimmt werden kann
        return Brushes.White;
    }

    // ConvertBack wird hier nicht benötigt, da die Farbe nicht zurück in einen bool gewandelt werden muss
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}