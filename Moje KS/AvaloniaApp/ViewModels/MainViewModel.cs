using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AvaloniaApp.Models;

namespace AvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ExerciseSession _session;

    public MainViewModel()
    {
        var exerciseModes = new[]
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

        _session = new ExerciseSession(exerciseModes);
        ExerciseModes = new ObservableCollection<string>(_session.ExerciseModes);
        SelectedExercise = _session.SelectedExercise;
        CurrentRound = _session.CurrentRound;
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
    private int _currentRound;

    [ObservableProperty]
    private bool _isParameterPanelExpanded = true;

    public string WindowTitle => $"Moje KS — {SelectedExercise}";

    [RelayCommand]
    private void SelectExercise(string exercise)
    {
        _session.SelectExercise(exercise);
        SelectedExercise = _session.SelectedExercise;
        CurrentRound = _session.CurrentRound;
        ShowAnswers = _session.ShowAnswers;
        StatusMessage = $"Wybrano tryb: {exercise}. Logika ćwiczenia zostanie podłączona w kolejnych krokach.";
    }

    [RelayCommand]
    private void GoPrevious()
    {
        _session.Previous();
        RefreshSessionState("Poprzedni ekran sesji ćwiczenia.");
    }

    [RelayCommand]
    private void GoNext()
    {
        _session.Next();
        RefreshSessionState("Następny ekran sesji ćwiczenia.");
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

    partial void OnShowAnswersChanged(bool value)
    {
        _session.SetShowAnswers(value);
        StatusMessage = value ? "Odpowiedzi są widoczne." : "Odpowiedzi są ukryte.";
    }

    private void RefreshSessionState(string message)
    {
        CurrentRound = _session.CurrentRound;
        ShowAnswers = _session.ShowAnswers;
        StatusMessage = $"{message} Runda: {CurrentRound}.";
    }
}
