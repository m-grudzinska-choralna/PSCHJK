using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaApp.Models;

public sealed class Ks2Element<T>
{
    public Ks2Element(string elementKind, int elementNumber, T element, object? answer)
    {
        ElementKind = elementKind;
        ElementNumber = elementNumber;
        Element = element;
        Answer = answer;
    }

    public string ElementKind { get; }
    public int ElementNumber { get; }
    public T Element { get; }
    public object? Answer { get; }
    public int AskedCount { get; set; }
    public int LastDrawIndex { get; set; } = -99999;
}

public abstract class Ks2ScreenContent
{
}

public sealed class Ks2RecognizeNoteScreenContent : Ks2ScreenContent
{
    public required MusicNote Note { get; init; }
}

public sealed class Ks2Screen
{
    public Ks2Screen(string mode, string exerciseKind, Ks2Element<MusicNote> element)
    {
        Mode = mode;
        ExerciseKind = exerciseKind;
        Element = element;
        Content = new Ks2RecognizeNoteScreenContent { Note = element.Element };
        Answer = element.Answer;
    }

    public string Mode { get; }
    public string ExerciseKind { get; }
    public Ks2Element<MusicNote> Element { get; }
    public Ks2ScreenContent Content { get; }
    public object? Answer { get; }
}

public sealed class Ks2NoteElementGenerator
{
    private readonly Random _random = new();
    private readonly IReadOnlyList<string> _rhythmicValueNames;

    public Ks2NoteElementGenerator(IReadOnlyList<int> noteNumbers, IReadOnlyList<string> chromaticSigns, IReadOnlyList<string> rhythmicValueNames)
    {
        _rhythmicValueNames = rhythmicValueNames.Count > 0 ? rhythmicValueNames : new[] { "cala_nuta" };
        var number = 0;
        var signs = chromaticSigns.Count > 0 ? chromaticSigns : new[] { "none" };
        foreach (var sign in signs)
        {
            foreach (var noteNumber in noteNumbers)
            {
                var note = new MusicNote(noteNumber, sign);
                AllElements.Add(new Ks2Element<MusicNote>("nuta", number++, note, note));
            }
        }
    }

    public List<Ks2Element<MusicNote>> AllElements { get; } = new();
    public List<Ks2Element<MusicNote>> DrawnElements { get; } = new();

    public Ks2Element<MusicNote> DrawNextElement(Ks2Element<MusicNote>? keepDistanceFrom = null)
    {
        if (AllElements.Count == 0)
        {
            throw new InvalidOperationException("No elements available.");
        }

        var minAskedCount = AllElements.Min(element => element.AskedCount);
        var previousElementNumber = DrawnElements.Count > 0 ? DrawnElements[^1].ElementNumber : -1;
        var candidateIndexes = new List<int>();

        for (var index = 0; index < AllElements.Count; index++)
        {
            var element = AllElements[index];
            if (element.AskedCount == minAskedCount
                && DrawnElements.Count - element.LastDrawIndex >= AllElements.Count / 2
                && index != previousElementNumber)
            {
                candidateIndexes.Add(index);
            }
        }

        var farCandidateIndexes = new List<int>();
        if ((DrawnElements.Count > 0 || keepDistanceFrom is not null) && candidateIndexes.Count > 0)
        {
            var referenceElement = keepDistanceFrom ?? DrawnElements[^1];
            var maxDistance = 0f;
            foreach (var index in candidateIndexes)
            {
                maxDistance = Math.Max(maxDistance, Distance(AllElements[index], referenceElement));
            }

            foreach (var index in candidateIndexes)
            {
                if (Distance(AllElements[index], referenceElement) > maxDistance / 2)
                {
                    farCandidateIndexes.Add(index);
                }
            }
        }

        Ks2Element<MusicNote> selected;
        if (farCandidateIndexes.Count > 1)
        {
            selected = AllElements[farCandidateIndexes[_random.Next(0, farCandidateIndexes.Count)]];
        }
        else if (candidateIndexes.Count >= 1)
        {
            selected = AllElements[candidateIndexes[_random.Next(0, candidateIndexes.Count)]];
        }
        else
        {
            selected = AllElements[_random.Next(0, AllElements.Count)];
        }

        selected.AskedCount++;
        selected.LastDrawIndex = DrawnElements.Count + 1;
        DrawnElements.Add(selected);
        var rhythm = RandomRhythmicValueName();
        var note = new MusicNote(selected.Element.Number, selected.Element.ChromaticSign, rhythm);
        return new Ks2Element<MusicNote>(selected.ElementKind, selected.ElementNumber, note, note)
        {
            AskedCount = selected.AskedCount,
            LastDrawIndex = selected.LastDrawIndex
        };
    }

    public string RandomRhythmicValueName()
    {
        return _rhythmicValueNames[_random.Next(0, _rhythmicValueNames.Count)];
    }

    private static float Distance(Ks2Element<MusicNote> left, Ks2Element<MusicNote> right)
    {
        return Math.Abs(right.Element.Number - left.Element.Number);
    }
}

public sealed class Ks2RecognizeNotesParameters
{
    public required bool Sequential { get; init; }
    public required string Clef { get; init; }
    public required IReadOnlyList<int> NoteNumbers { get; init; }
    public required IReadOnlyList<string> ChromaticSigns { get; init; }
    public required IReadOnlyList<string> RhythmicValues { get; init; }

    public string BuildKey()
    {
        return $"{Sequential}|{Clef}|{string.Join(",", NoteNumbers)}|{string.Join(",", ChromaticSigns)}|{string.Join(",", RhythmicValues)}";
    }
}

public sealed class Ks2ScreenNavigator
{
    private readonly Ks2RecognizeNotesData _data;
    private readonly bool _sequential;
    private readonly List<Ks2Screen> _screens;
    private int _index;

    public Ks2ScreenNavigator(Ks2RecognizeNotesData data, bool sequential)
    {
        _data = data;
        _sequential = sequential;
        _screens = sequential ? new List<Ks2Screen>(data.SequentialScreens) : new List<Ks2Screen> { data.NextScreen() };
    }

    public bool IsAnswerShown { get; private set; }
    public Ks2Screen Current => _screens[_index];

    public void Next(bool showAnswers)
    {
        var newScreen = true;
        if (showAnswers)
        {
            newScreen = IsAnswerShown;
            IsAnswerShown = !IsAnswerShown;
        }

        if (!newScreen)
        {
            return;
        }

        if (_sequential)
        {
            _index = _index < _screens.Count - 1 ? _index + 1 : 0;
        }
        else if (_index < _screens.Count - 1)
        {
            _index++;
            IsAnswerShown = _index != _screens.Count - 1 && showAnswers;
        }
        else
        {
            _screens.Add(_data.NextScreen());
            _index = _screens.Count - 1;
        }
    }

    public void Previous(bool showAnswers)
    {
        if (_sequential)
        {
            var goBack = true;
            if (showAnswers)
            {
                goBack = IsAnswerShown;
                IsAnswerShown = !IsAnswerShown;
            }

            if (goBack)
            {
                _index = _index > 0 ? _index - 1 : _screens.Count - 1;
            }
        }
        else if (_index >= 1)
        {
            _index--;
            if (showAnswers)
            {
                IsAnswerShown = true;
            }
        }
    }

    public void HideAnswer()
    {
        IsAnswerShown = false;
    }
}

public sealed class Ks2RecognizeNotesData
{
    private readonly Ks2RecognizeNotesParameters _parameters;
    private readonly Ks2NoteElementGenerator _generator;
    private readonly List<Ks2Screen> _screens = new();
    private int _screenIndex;

    public Ks2RecognizeNotesData(Ks2RecognizeNotesParameters parameters)
    {
        _parameters = parameters;
        _generator = new Ks2NoteElementGenerator(parameters.NoteNumbers, parameters.ChromaticSigns, parameters.RhythmicValues);
        if (_parameters.Sequential)
        {
            foreach (var element in _generator.AllElements)
            {
                var note = new MusicNote(element.Element.Number, element.Element.ChromaticSign, _generator.RandomRhythmicValueName());
                var sequentialElement = new Ks2Element<MusicNote>(element.ElementKind, element.ElementNumber, note, note);
                _screens.Add(new Ks2Screen("rozpoznawanie", "nuty", sequentialElement));
            }
        }
    }

    public IReadOnlyList<Ks2Screen> SequentialScreens => _screens;

    public Ks2Screen NextScreen()
    {
        if (_parameters.Sequential)
        {
            if (_screens.Count == 0)
            {
                throw new InvalidOperationException("No sequential screens available.");
            }

            var screen = _screens[_screenIndex];
            _screenIndex = (_screenIndex + 1) % _screens.Count;
            return screen;
        }

        var element = _generator.DrawNextElement();
        return new Ks2Screen("rozpoznawanie", "nuty", element);
    }
}
