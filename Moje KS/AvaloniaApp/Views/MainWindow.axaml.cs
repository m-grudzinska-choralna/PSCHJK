using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaApp.ViewModels;

namespace AvaloniaApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is not MainViewModel viewModel)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.Left:
                viewModel.GoPreviousCommand.Execute(null);
                e.Handled = true;
                break;
            case Key.Right:
                viewModel.GoNextCommand.Execute(null);
                e.Handled = true;
                break;
            case Key.Space:
                viewModel.ShowAnswers = !viewModel.ShowAnswers;
                e.Handled = true;
                break;
            case Key.F2:
                viewModel.ToggleParameterPanelCommand.Execute(null);
                e.Handled = true;
                break;
            case Key.Escape:
                Close();
                e.Handled = true;
                break;
        }
    }
}