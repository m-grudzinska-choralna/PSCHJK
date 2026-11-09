using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaApp.Models;

public sealed class NoteQuestionGenerator
{
    private readonly Random _random = new();

    public NoteQuestion Create(NoteRangeSettings settings)
    {
        var notes = GetCandidateNotes(settings);
        var note = notes[_random.Next(notes.Count)];
        var clef = settings.Clef == "basowy" ? "basowy" : "wiolinowy";
        return new NoteQuestion(note, clef, note);
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
