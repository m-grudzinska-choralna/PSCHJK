using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaApp.Models;

public sealed class NoteQuestionGenerator
{
    private readonly Random _random = new();
    private int _sequentialIndex;
    private string _sequenceKey = string.Empty;
    private readonly Dictionary<string, int> _randomUsageCounts = new(StringComparer.Ordinal);
    private string _randomKey = string.Empty;
    private string _lastRandomNote = string.Empty;

    public NoteQuestion Create(NoteRangeSettings settings, bool sequential)
    {
        var notes = GetCandidateNotes(settings);
        var note = sequential ? GetNextSequentialNote(settings, notes) : GetNextBalancedRandomNote(settings, notes);
        var clef = settings.Clef == "basowy" ? "basowy" : "wiolinowy";
        return new NoteQuestion(note, clef, note);
    }

    public void ResetSequence()
    {
        _sequentialIndex = 0;
        _sequenceKey = string.Empty;
        _randomUsageCounts.Clear();
        _randomKey = string.Empty;
        _lastRandomNote = string.Empty;
    }

    private string GetNextBalancedRandomNote(NoteRangeSettings settings, IReadOnlyList<string> notes)
    {
        var key = BuildSequenceKey(settings);
        if (!string.Equals(_randomKey, key, StringComparison.Ordinal))
        {
            _randomKey = key;
            _randomUsageCounts.Clear();
            _lastRandomNote = string.Empty;
        }

        var selectableNotes = notes;
        if (notes.Count > 1 && !string.IsNullOrEmpty(_lastRandomNote))
        {
            selectableNotes = notes.Where(note => !string.Equals(note, _lastRandomNote, StringComparison.Ordinal)).ToList();
        }

        var minUsage = int.MaxValue;
        foreach (var note in selectableNotes)
        {
            if (!_randomUsageCounts.TryGetValue(note, out var usage))
            {
                usage = 0;
            }

            if (usage < minUsage)
            {
                minUsage = usage;
            }
        }

        var candidates = new List<string>();
        foreach (var note in selectableNotes)
        {
            if (!_randomUsageCounts.TryGetValue(note, out var usage))
            {
                usage = 0;
            }

            if (usage == minUsage)
            {
                candidates.Add(note);
            }
        }

        var selectedNote = candidates[_random.Next(candidates.Count)];
        _randomUsageCounts[selectedNote] = minUsage + 1;
        _lastRandomNote = selectedNote;
        return selectedNote;
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
