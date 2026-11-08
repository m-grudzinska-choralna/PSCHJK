using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KS2;

namespace MojeKS.AvaloniaApp
{
    public partial class MainWindow : Window
    {
        public string TonacjaDisplay { get; }

        public MainWindow()
        {
            TonacjaDisplay = new Tonacja("C-dur").ToString();
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
