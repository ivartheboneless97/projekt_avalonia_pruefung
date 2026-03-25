using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading; // WICHTIG für Dispatcher
using Avalonia.VisualTree;
using System.Linq;

namespace projekt_avalonia_pruefung_master.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void BetragInput_GotFocus(object? sender, GotFocusEventArgs e)
    {
        var numericUpDown = sender as NumericUpDown;
        if (numericUpDown != null)
        {
            // Wir führen den Code "Asynchron" aus, damit Avalonia Zeit hat
            Dispatcher.UIThread.Post(() =>
            {
                var textBox = numericUpDown.GetVisualDescendants()
                                           .OfType<TextBox>()
                                           .FirstOrDefault();
                if (textBox != null)
                {
                    textBox.SelectAll();
                    // Optional: Fokus erzwingen
                    textBox.Focus();
                }
            }, DispatcherPriority.Background);
        }
    }
}
