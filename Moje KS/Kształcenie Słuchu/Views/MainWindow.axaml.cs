using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Input;
using Avalonia.Interactivity;
using KsztalcenieSluchu.ViewModels;

namespace KsztalcenieSluchu.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
        AddHandler(KeyUpEvent, OnKeyUp, RoutingStrategies.Tunnel);
        Opened += OnOpened;
    }

    private async void OnOpened(object? sender, EventArgs e)
    {
        Opened -= OnOpened;

        var okButton = new Button
        {
            Content = "OK",
            HorizontalAlignment = HorizontalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            MinWidth = 110,
            Padding = new Thickness(16, 8),
            CornerRadius = new CornerRadius(20),
            Background = new SolidColorBrush(Color.Parse("#C62828")),
            Foreground = Brushes.White,
            FontWeight = FontWeight.SemiBold
        };

        var header = new Border
        {
            Background = new LinearGradientBrush
            {
                StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                EndPoint = new RelativePoint(1, 0, RelativeUnit.Relative),
                GradientStops =
                {
                    new GradientStop(Color.Parse("#B71C1C"), 0),
                    new GradientStop(Color.Parse("#E53935"), 1)
                }
            },
            Padding = new Thickness(24, 4),
            Child = new TextBlock
            {
                Text = "⚠  Uwaga!",
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White,
                FontFamily = new FontFamily("Arial"),
                FontSize = 16,
                FontWeight = FontWeight.Bold
            }
        };

        var body = new StackPanel
        {
            Margin = new Thickness(28, 22, 28, 24),
            Spacing = 14,
            Children =
            {
                new TextBlock
                {
                    Text = "Niniejszy program powstał na WYŁĄCZNY użytek\nMarty Grudzińskiej i osób przez nią upoważnionych.",
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.Parse("#8E1B1B")),
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 18,
                    FontWeight = FontWeight.SemiBold,
                    TextWrapping = TextWrapping.Wrap
                },
                new TextBlock
                {
                    Text = "Poznańska Szkoła Chóralna Jerzego Kurczewskiego, 2026",
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.Parse("#D84343")),
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 15,
                    FontStyle = FontStyle.Italic,
                    TextWrapping = TextWrapping.Wrap
                },
                okButton
            }
        };

        var dialog = new Window
        {
            Title = "Uwaga",
            Width = 560,
            SizeToContent = SizeToContent.Height,
            CanResize = false,
            SystemDecorations = SystemDecorations.BorderOnly,
            ExtendClientAreaToDecorationsHint = true,
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaTitleBarHeightHint = -1,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Background = new SolidColorBrush(Color.Parse("#FFEBEE")),
            Content = new StackPanel { Children = { header, body } }
        };

        okButton.Click += (_, _) => dialog.Close();
        header.PointerPressed += (_, args) =>
        {
            if (args.GetCurrentPoint(dialog).Properties.IsLeftButtonPressed)
            {
                dialog.BeginMoveDrag(args);
            }
        };
        await dialog.ShowDialog(this);
    }

    private void OnMinimizeClick(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void OnMaximizeClick(object? sender, RoutedEventArgs e) =>
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

    private void OnCloseClick(object? sender, RoutedEventArgs e) => Close();

    private void OnTitleBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Button || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            return;
        }

        BeginMoveDrag(e);
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space)
        {
            e.Handled = true;
        }
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
                viewModel.HandleSpaceCommand.Execute(null);
                e.Handled = true;
                break;
            case Key.Escape:
                Close();
                e.Handled = true;
                break;
        }
    }
}