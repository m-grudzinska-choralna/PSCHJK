using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaApp.Models;

public sealed class NoteQuestionGenerator
{
    private readonly Random _random = new();
    private int _sequentialIndex;
    private string _sequenceKey = string.Empty;

    public NoteQuestion Create(NoteRangeSettings settings, bool sequential)
    {
        var notes = GetCandidateNotes(settings);
        var note = sequential ? GetNextSequentialNote(settings, notes) : notes[_random.Next(notes.Count)];
        var clef = settings.Clef == "basowy" ? "basowy" : "wiolinowy";
        return new NoteQuestion(note, clef, note);
    }

    public void ResetSequence()
    {
        _sequentialIndex = 0;
        _sequenceKey = string.Empty;
    }

    private string GetNextSequentialNote(NoteRangeSettings settings, IReadOnlyList<string> notes)
    {
        var key = BuildSequenceKey(settings);
        if (!string.Equals(_sequenceKey, key, StringComparison.Ordinal))
        {
            _sequenceKey = key;
            _sequentialIndex = 0;
        }

        if (_sequentialIndex >= notes.Count)
        {
            _sequentialIndex = 0;
        }

        var note = notes[_sequentialIndex];
        _sequentialIndex = (_sequentialIndex + 1) % notes.Count;
        return note;
    }

    private static string BuildSequenceKey(NoteRangeSettings settings)
    {
        var clef = settings.Clef == "basowy" ? "basowy" : "wiolinowy";
        var from = clef == "basowy" ? settings.BassFrom : settings.TrebleFrom;
        var to = clef == "basowy" ? settings.BassTo : settings.TrebleTo;
        return $"{clef}:{from}:{to}";
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
