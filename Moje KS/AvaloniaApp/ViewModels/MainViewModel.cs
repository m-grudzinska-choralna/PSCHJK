using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AvaloniaApp.Models;

namespace AvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ExerciseSession _session;
    private readonly NoteRangeSettings _noteRange;

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
        _noteRange = new NoteRangeSettings();
        ExerciseModes = new ObservableCollection<string>(_session.ExerciseModes);
        Clefs = new ObservableCollection<string>(NoteRangeSettings.Clefs);
        Notes = new ObservableCollection<string>(NoteRangeSettings.Notes);
        SelectedExercise = _session.SelectedExercise;
        CurrentRound = _session.CurrentRound;
        SelectedClef = _noteRange.Clef;
        TrebleFrom = _noteRange.TrebleFrom;
        TrebleTo = _noteRange.TrebleTo;
        BassFrom = _noteRange.BassFrom;
        BassTo = _noteRange.BassTo;
        StatusMessage = "Gotowe do rozpoczęcia ćwiczenia.";
    }

    public ObservableCollection<string> ExerciseModes { get; }
    public ObservableCollection<string> Clefs { get; }
    public ObservableCollection<string> Notes { get; }

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

    [ObservableProperty]
    private string _selectedClef;

    [ObservableProperty]
    private string _trebleFrom;

    [ObservableProperty]
    private string _trebleTo;

    [ObservableProperty]
    private string _bassFrom;

    [ObservableProperty]
    private string _bassTo;

    public string WindowTitle => $"Moje KS — {SelectedExercise}";

    public string NoteRangeStatus => $"Klucz: {SelectedClef}; wiolinowy: {TrebleFrom}–{TrebleTo}; basowy: {BassFrom}–{BassTo}";

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
    private void ApplyNoteRange()
    {
        try
        {
            _noteRange.Update(SelectedClef, TrebleFrom, TrebleTo, BassFrom, BassTo);
            OnPropertyChanged(nameof(NoteRangeStatus));
            StatusMessage = "Zakres nut został zapisany w sesji.";
        }
        catch (ArgumentException exception)
        {
            StatusMessage = exception.Message;
        }
    }

    [RelayCommand]
    private void GoPrevious()
    {
        _session.Previous();
        RefreshSessionState("Poprzedni ekran sesji ćwiczenia.");
    }

    partial void OnSelectedClefChanged(string value) => OnPropertyChanged(nameof(NoteRangeStatus));
    partial void OnTrebleFromChanged(string value) => OnPropertyChanged(nameof(NoteRangeStatus));
    partial void OnTrebleToChanged(string value) => OnPropertyChanged(nameof(NoteRangeStatus));
    partial void OnBassFromChanged(string value) => OnPropertyChanged(nameof(NoteRangeStatus));
    partial void OnBassToChanged(string value) => OnPropertyChanged(nameof(NoteRangeStatus));

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
