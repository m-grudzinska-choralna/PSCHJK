using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AvaloniaApp.Models;

namespace AvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ExerciseSession _session;
    private readonly NoteRangeSettings _noteRange;
    private readonly NoteQuestionGenerator _questionGenerator = new();
    private bool _isInitialized;

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
        Notes = new ObservableCollection<string>(NoteRangeSettings.Notes);
        TrebleNotes = new ObservableCollection<string>(NoteRangeSettings.TrebleNotes);
        BassNotes = new ObservableCollection<string>(NoteRangeSettings.BassNotes);
        SelectedExercise = _session.SelectedExercise;
        CurrentRound = _session.CurrentRound;
        SelectedClef = _noteRange.Clef;
        TrebleFrom = _noteRange.TrebleFrom;
        TrebleTo = _noteRange.TrebleTo;
        BassFrom = _noteRange.BassFrom;
        BassTo = _noteRange.BassTo;
        CurrentQuestion = _questionGenerator.Create(_noteRange);
        _isInitialized = true;
        StatusMessage = "Gotowe do rozpoczęcia ćwiczenia.";
    }

    public ObservableCollection<string> Notes { get; }
    public ObservableCollection<string> TrebleNotes { get; }
    public ObservableCollection<string> BassNotes { get; }

    [ObservableProperty]
    private string _selectedExercise;

    [ObservableProperty]
    private string _statusMessage;

    [ObservableProperty]
    private bool _showAnswers;

    [ObservableProperty]
    private bool _isAnswerVisible;

    [ObservableProperty]
    private int _currentRound;

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

    public string SymbolicNote => NoteRangeSettings.ToSymbolicName(CurrentQuestion.Note);
    public bool IsTrebleClefSelected
    {
        get => SelectedClef == "wiolinowy";
        set
        {
            if (value)
            {
                SelectedClef = "wiolinowy";
            }
        }
    }

    public bool IsBassClefSelected
    {
        get => SelectedClef == "basowy";
        set
        {
            if (value)
            {
                SelectedClef = "basowy";
            }
        }
    }

    [ObservableProperty]
    private NoteQuestion _currentQuestion;

    partial void OnCurrentQuestionChanged(NoteQuestion value) => OnPropertyChanged(nameof(SymbolicNote));

    [RelayCommand]
    private void SelectExercise(string exercise)
    {
        _session.SelectExercise(exercise);
        if (exercise == "Rozpoznawanie nut")
        {
            CreateNewQuestion();
        }
        RefreshSessionState($"Wybrano tryb: {exercise}.");
    }

    private void ApplyNoteRange()
    {
        try
        {
            _noteRange.Update(SelectedClef, TrebleFrom, TrebleTo, BassFrom, BassTo);
            CreateNewQuestion();
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
        CreateNewQuestion();
        RefreshSessionState("Poprzedni ekran sesji ćwiczenia.");
    }

    partial void OnSelectedClefChanged(string value)
    {
        OnPropertyChanged(nameof(IsTrebleClefSelected));
        OnPropertyChanged(nameof(IsBassClefSelected));
        if (_isInitialized)
        {
            ApplyNoteRange();
        }
    }
    partial void OnTrebleFromChanged(string value) => ApplyNoteRangeIfInitialized();
    partial void OnTrebleToChanged(string value) => ApplyNoteRangeIfInitialized();
    partial void OnBassFromChanged(string value) => ApplyNoteRangeIfInitialized();
    partial void OnBassToChanged(string value) => ApplyNoteRangeIfInitialized();

    private void ApplyNoteRangeIfInitialized()
    {
        if (_isInitialized)
        {
            ApplyNoteRange();
        }
    }

    [RelayCommand]
    private void GoNext()
    {
        _session.Next();
        CreateNewQuestion();
        RefreshSessionState("Następny ekran sesji ćwiczenia.");
    }

    [RelayCommand]
    private void GenerateNewNote()
    {
        CreateNewQuestion();
    }

    [RelayCommand]
    private void HandleSpace()
    {
        if (!ShowAnswers)
        {
            CreateNewQuestion();
            return;
        }

        if (!IsAnswerVisible)
        {
            IsAnswerVisible = true;
            return;
        }

        CreateNewQuestion();
    }

    partial void OnSelectedExerciseChanged(string value)
    {
        OnPropertyChanged(nameof(WindowTitle));

        if (_session.ExerciseModes.Contains(value, StringComparer.Ordinal))
        {
            _session.SelectExercise(value);
            CurrentRound = _session.CurrentRound;
            ShowAnswers = _session.ShowAnswers;
            StatusMessage = $"Wybrano tryb: {value}.";
        }
    }

    partial void OnShowAnswersChanged(bool value)
    {
        _session.SetShowAnswers(value);
        if (!value)
        {
            IsAnswerVisible = false;
        }
        StatusMessage = value ? "Odpowiedzi są widoczne." : "Odpowiedzi są ukryte.";
    }

    private void CreateNewQuestion()
    {
        CurrentQuestion = _questionGenerator.Create(_noteRange);
        IsAnswerVisible = false;
    }

    private void RefreshSessionState(string message)
    {
        CurrentRound = _session.CurrentRound;
        ShowAnswers = _session.ShowAnswers;
        StatusMessage = $"{message} Runda: {CurrentRound}.";
    }
}
