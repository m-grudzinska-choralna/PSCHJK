using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaApp.Models;

/// <summary>
/// Framework-neutral note range settings used by the Avalonia exercise screen.
/// </summary>
public sealed class NoteRangeSettings
{
    public static IReadOnlyList<string> Clefs { get; } = new[]
    {
        "wiolinowy",
        "basowy",
        "wiolinowy i basowy"
    };

    public static IReadOnlyList<string> Notes { get; } = new[]
    {
        "C3", "D3", "E3", "F3", "G3", "A3", "B3",
        "C4", "D4", "E4", "F4", "G4", "A4", "B4",
        "C5", "D5", "E5", "F5", "G5", "A5", "B5"
    };

    public string Clef { get; private set; } = Clefs[0];

    public string TrebleFrom { get; private set; } = "C4";

    public string TrebleTo { get; private set; } = "C5";

    public string BassFrom { get; private set; } = "C3";

    public string BassTo { get; private set; } = "C4";

    public void Update(string clef, string trebleFrom, string trebleTo, string bassFrom, string bassTo)
    {
        if (!Clefs.Contains(clef, StringComparer.Ordinal))
        {
            throw new ArgumentException("Unknown clef.", nameof(clef));
        }

        if (!Notes.Contains(trebleFrom, StringComparer.Ordinal) ||
            !Notes.Contains(trebleTo, StringComparer.Ordinal) ||
            !Notes.Contains(bassFrom, StringComparer.Ordinal) ||
            !Notes.Contains(bassTo, StringComparer.Ordinal))
        {
            throw new ArgumentException("The selected note is not available.");
        }

        if (IndexOf(trebleFrom) >= IndexOf(trebleTo))
        {
            throw new ArgumentException("Treble range must contain at least two notes.");
        }

        if (IndexOf(bassFrom) >= IndexOf(bassTo))
        {
            throw new ArgumentException("Bass range must contain at least two notes.");
        }

        Clef = clef;
        TrebleFrom = trebleFrom;
        TrebleTo = trebleTo;
        BassFrom = bassFrom;
        BassTo = bassTo;
    }

    private static int IndexOf(string note) => Array.IndexOf(Notes.ToArray(), note);
}
