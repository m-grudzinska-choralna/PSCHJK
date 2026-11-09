using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        ExerciseModes = new ObservableCollection<string>
        {
            "Rozpoznawanie nut",
            "Rozpoznawanie symboli",
            "Rozpoznawanie klawiszy",
            "Rozpoznawanie interwałów",
            "Losowe nuty",
            "Losowe interwały",
            "Tonacje",
            "Edytor nut",
            "Edytor interwałów"
        };

        SelectedExercise = ExerciseModes[0];
        StatusMessage = "Gotowe do rozpoczęcia ćwiczenia.";
    }

    public ObservableCollection<string> ExerciseModes { get; }

    [ObservableProperty]
    private string _selectedExercise;

    [ObservableProperty]
    private string _statusMessage;

    [ObservableProperty]
    private bool _showAnswers;

    [ObservableProperty]
    private bool _isParameterPanelExpanded = true;

    public string WindowTitle => $"Moje KS — {SelectedExercise}";

    [RelayCommand]
    private void SelectExercise(string exercise)
    {
        SelectedExercise = exercise;
        StatusMessage = $"Wybrano tryb: {exercise}. Logika ćwiczenia zostanie podłączona w kolejnych krokach.";
    }

    [RelayCommand]
    private void GoPrevious()
    {
        StatusMessage = "Poprzedni ekran — nawigacja zostanie podłączona do sesji ćwiczenia.";
    }

    [RelayCommand]
    private void GoNext()
    {
        StatusMessage = "Następny ekran — nawigacja zostanie podłączona do sesji ćwiczenia.";
    }

    [RelayCommand]
    private void ToggleParameterPanel()
    {
        IsParameterPanelExpanded = !IsParameterPanelExpanded;
    }

    partial void OnSelectedExerciseChanged(string value)
    {
        OnPropertyChanged(nameof(WindowTitle));
    }
}
