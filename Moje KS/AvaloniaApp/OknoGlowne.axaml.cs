using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace MojeKS.AvaloniaApp
{
    public partial class OknoGlowne : UserControl
    {
        private int currentRound = 1;

        public OknoGlowne()
        {
            InitializeComponent();

            // Wire up simple button handlers for PoC
            var back = this.FindControl<Button>("Wstecz_button");
            var next = this.FindControl<Button>("dalej_button");
            if (back != null) back.Click += OnWsteczClick;
            if (next != null) next.Click += OnDalejClick;

            UpdateStatusLabels();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnWsteczClick(object? sender, RoutedEventArgs e)
        {
            if (currentRound > 1) currentRound--;
            UpdateStatusLabels();
        }

        private void OnDalejClick(object? sender, RoutedEventArgs e)
        {
            currentRound++;
            UpdateStatusLabels();
        }

        private void UpdateStatusLabels()
        {
            var bas = this.FindControl<TextBlock>("basowy_alert_label");
            var wiol = this.FindControl<TextBlock>("wiolowy_alert_label");
            if (bas != null) bas.Text = $"Runda: {currentRound}";
            if (wiol != null) wiol.Text = $"Stan: { (currentRound % 2 == 0 ? "OK" : "Czekaj") }";
        }
    }
}

