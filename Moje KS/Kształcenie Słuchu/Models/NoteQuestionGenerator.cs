using System;
using System.Collections.Generic;
using System.Linq;

namespace KsztalcenieSluchu.Models;

public sealed class NoteQuestionGenerator
{
    private string _ks2SessionKey = string.Empty;
    private Ks2ScreenNavigator? _navigator;

    public NoteQuestion Create(NoteRangeSettings settings, bool sequential)
    {
        return Build(settings, sequential, navigator => { });
    }

    public NoteQuestion Next(NoteRangeSettings settings, bool sequential, bool showAnswers)
    {
        return Build(settings, sequential, navigator => navigator.Next(showAnswers));
    }

    public NoteQuestion Previous(NoteRangeSettings settings, bool sequential, bool showAnswers)
    {
        return Build(settings, sequential, navigator => navigator.Previous(showAnswers));
    }

    public bool IsAnswerShown => _navigator?.IsAnswerShown ?? false;

    private NoteQuestion Build(NoteRangeSettings settings, bool sequential, Action<Ks2ScreenNavigator> move)
    {
        var parameters = BuildKs2Parameters(settings, sequential);
        var key = parameters.BuildKey();
        if (!string.Equals(_ks2SessionKey, key, StringComparison.Ordinal) || _navigator is null)
        {
            _ks2SessionKey = key;
            _navigator = new Ks2ScreenNavigator(new Ks2RecognizeNotesData(parameters), sequential);
        }

        move(_navigator);
        var screen = _navigator.Current;
        var clef = settings.Clef == "basowy" ? "basowy" : "wiolinowy";
        return new NoteQuestion(screen.Element.Element, clef, screen);
    }

    public void ResetSequence()
    {
        _ks2SessionKey = string.Empty;
        _navigator = null;
    }

    private static Ks2RecognizeNotesParameters BuildKs2Parameters(NoteRangeSettings settings, bool sequential)
    {
        var notes = GetCandidateNotes(settings)
            .Select(NoteRangeSettings.NoteNumber)
            .Where(number => number >= 0)
            .ToList();
        if (notes.Count == 0)
        {
            notes.Add(0);
        }

        return new Ks2RecognizeNotesParameters
        {
            Sequential = sequential,
            Clef = settings.Clef == "basowy" ? "bas" : "wiol",
            NoteNumbers = notes,
            ChromaticSigns = settings.SelectedChromaticSigns,
            RhythmicValues = settings.SelectedRhythmicValues
        };
    }

    private static IReadOnlyList<string> GetCandidateNotes(NoteRangeSettings settings)
    {
        var from = settings.Clef == "basowy" ? settings.BassFrom : settings.TrebleFrom;
        var to = settings.Clef == "basowy" ? settings.BassTo : settings.TrebleTo;
        var fromIndex = Array.IndexOf(NoteRangeSettings.Notes.ToArray(), from);
        var toIndex = Array.IndexOf(NoteRangeSettings.Notes.ToArray(), to);

        if (fromIndex < 0 || toIndex <= fromIndex)
        {
            return new[] { from };
        }

        var result = new List<string>();
        for (var index = fromIndex; index <= toIndex; index++)
        {
            result.Add(NoteRangeSettings.Notes[index]);
        }

        return result;
    }
}
