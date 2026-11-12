using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaApp.Models;

/// <summary>
/// Framework-neutral state of the active exercise session.
/// It deliberately does not reference Avalonia or Windows Forms.
/// </summary>
public sealed class ExerciseSession
{
    private readonly IReadOnlyList<string> _exerciseModes;

    public ExerciseSession(IReadOnlyList<string> exerciseModes)
    {
        if (exerciseModes.Count == 0)
        {
            throw new ArgumentException("At least one exercise mode is required.", nameof(exerciseModes));
        }

        _exerciseModes = exerciseModes;
        SelectedExercise = exerciseModes[0];
        CurrentRound = 1;
    }

    public IReadOnlyList<string> ExerciseModes => _exerciseModes;

    public string SelectedExercise { get; private set; }

    public int CurrentRound { get; private set; }

    public bool ShowAnswers { get; private set; }

    public void SelectExercise(string exercise)
    {
        if (!_exerciseModes.Contains(exercise, StringComparer.Ordinal))
        {
            throw new ArgumentException("The selected exercise is not available.", nameof(exercise));
        }

        SelectedExercise = exercise;
        CurrentRound = 1;
        ShowAnswers = false;
    }

    public void Next()
    {
        CurrentRound++;
    }

    public void Previous()
    {
        if (CurrentRound > 1)
        {
            CurrentRound--;
        }
    }

    public void SetShowAnswers(bool showAnswers)
    {
        ShowAnswers = showAnswers;
    }
}
