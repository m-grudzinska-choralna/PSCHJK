using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AvaloniaApp.Models;

public enum NoteNameMode
{
    Letter,
    Solmization
}

/// <summary>
/// Framework-neutral note range settings used by the Avalonia exercise screen.
/// </summary>
public sealed class NoteRangeSettings
{
    private const int LowestNoteNumber = 0;
    private const int HighestNoteNumber = 44;
    private const int TrebleLowestNoteNumber = 20;
    private const int TrebleHighestNoteNumber = 44;
    private const int BassLowestNoteNumber = 0;
    private const int BassHighestNoteNumber = 26;

    private static readonly string[] NoteNames = { "c", "d", "e", "f", "g", "a", "h" };
    private static readonly string[] SolmizationNoteNames = { "do", "re", "mi", "fa", "sol", "la", "si" };
    private static readonly string[] OctaveNames =
    {
        "subkontra", "kontra", "wielkie", "małe", "razkreślne",
        "dwukreślne", "trzykreślne", "czterokreślne", "pięciokreślne"
    };

    public static IReadOnlyList<string> Clefs { get; } = new[]
    {
        "wiolinowy",
        "basowy"
    };

    public static IReadOnlyList<string> ChromaticSigns { get; } = new[]
    {
        "none",
        "flat",
        "sharp",
        "double-flat",
        "double-sharp"
    };

    public static IReadOnlyList<string> RhythmicValueNames { get; } = new[]
    {
        "cala_nuta",
        "polnuta",
        "cwiercnuta",
        "osemka",
        "szesnastka"
    };

    public static IReadOnlyList<string> Notes { get; } = CreateNoteList(LowestNoteNumber, HighestNoteNumber);

    public static IReadOnlyList<string> TrebleNotes { get; } = CreateNoteList(TrebleLowestNoteNumber, TrebleHighestNoteNumber);

    public static IReadOnlyList<string> BassNotes { get; } = CreateNoteList(BassLowestNoteNumber, BassHighestNoteNumber);

    public string Clef { get; private set; } = Clefs[0];

    public string TrebleFrom { get; private set; } = FormatNote(23);

    public string TrebleTo { get; private set; } = FormatNote(30);

    public string BassFrom { get; private set; } = FormatNote(8);

    public string BassTo { get; private set; } = FormatNote(20);

    public IReadOnlyList<string> SelectedChromaticSigns { get; private set; } = new[] { ChromaticSigns[0] };

    public IReadOnlyList<string> SelectedRhythmicValues { get; private set; } = new[] { RhythmicValueNames[0] };

    public void Update(
        string clef,
        string trebleFrom,
        string trebleTo,
        string bassFrom,
        string bassTo,
        IReadOnlyList<string> chromaticSigns,
        IReadOnlyList<string> rhythmicValues)
    {
        if (!Clefs.Contains(clef, StringComparer.Ordinal))
        {
            throw new ArgumentException("Unknown clef.", nameof(clef));
        }

        if (!TrebleNotes.Contains(trebleFrom, StringComparer.Ordinal) ||
            !TrebleNotes.Contains(trebleTo, StringComparer.Ordinal) ||
            !BassNotes.Contains(bassFrom, StringComparer.Ordinal) ||
            !BassNotes.Contains(bassTo, StringComparer.Ordinal))
        {
            throw new ArgumentException("The selected note is not available.");
        }

        if (NoteNumber(trebleFrom) >= NoteNumber(trebleTo))
        {
            throw new ArgumentException("Treble range must contain at least two notes.");
        }

        if (NoteNumber(bassFrom) >= NoteNumber(bassTo))
        {
            throw new ArgumentException("Bass range must contain at least two notes.");
        }

        if (chromaticSigns.Count == 0 || chromaticSigns.Any(sign => !ChromaticSigns.Contains(sign, StringComparer.Ordinal)))
        {
            throw new ArgumentException("Wybierz co najmniej jeden znak chromatyczny.", nameof(chromaticSigns));
        }

        if (rhythmicValues.Count == 0 || rhythmicValues.Any(value => !RhythmicValueNames.Contains(value, StringComparer.Ordinal)))
        {
            throw new ArgumentException("Wybierz co najmniej jedną wartość rytmiczną.", nameof(rhythmicValues));
        }

        Clef = clef;
        TrebleFrom = trebleFrom;
        TrebleTo = trebleTo;
        BassFrom = bassFrom;
        BassTo = bassTo;
        SelectedChromaticSigns = ChromaticSigns.Where(sign => chromaticSigns.Contains(sign)).ToList();
        SelectedRhythmicValues = RhythmicValueNames.Where(value => rhythmicValues.Contains(value)).ToList();
    }

    public static string ToAccidentalSymbol(string chromaticSign)
    {
        return chromaticSign switch
        {
            "flat" => "♭",
            "sharp" => "♯",
            "double-flat" => "♭♭",
            "double-sharp" => "𝄪",
            _ => string.Empty
        };
    }

    public static int NoteNumber(string note)
    {
        for (var number = LowestNoteNumber; number <= HighestNoteNumber; number++)
        {
            if (string.Equals(FormatNote(number), note, StringComparison.Ordinal))
            {
                return number;
            }
        }

        var normalizedNote = note?.Trim();
        if (string.IsNullOrEmpty(normalizedNote))
        {
            return -1;
        }

        var noteIndex = Array.IndexOf(NoteNames, char.ToLowerInvariant(normalizedNote[0]).ToString());
        if (noteIndex >= 0 && int.TryParse(normalizedNote.AsSpan(1), NumberStyles.Integer, CultureInfo.InvariantCulture, out var octaveNumber))
        {
            var number = (long)octaveNumber * NoteNames.Length + noteIndex - 5;
            if (number >= LowestNoteNumber && number <= HighestNoteNumber)
            {
                return (int)number;
            }
        }

        return -1;
    }

    public static string ToSymbolicName(string note, NoteNameMode mode = NoteNameMode.Letter)
    {
        var number = NoteNumber(note);
        if (number < 0)
        {
            return note;
        }

        var octaveNumber = (number + 5) / 7;
        var noteName = mode == NoteNameMode.Solmization
            ? SolmizationNoteNames[(number + 5) % 7]
            : NoteNames[(number + 5) % 7];

        if (mode == NoteNameMode.Letter)
        {
            if (octaveNumber == 2)
            {
                return noteName.ToUpperInvariant();
            }

            if (octaveNumber <= 1)
            {
                return noteName.ToUpperInvariant();
            }

            var octaveSuffix = octaveNumber >= 4 ? "⁰¹²³⁴⁵⁶⁷⁸⁹"[octaveNumber - 3].ToString() : string.Empty;
            return noteName + octaveSuffix;
        }

        return noteName;
    }

    public static string ToDisplayName(string note, string chromaticSign, NoteNameMode mode = NoteNameMode.Letter)
    {
        var symbolicName = ToSymbolicName(note, mode);
        if (mode != NoteNameMode.Letter)
        {
            return symbolicName;
        }

        var number = NoteNumber(note);
        if (number < 0)
        {
            return symbolicName;
        }

        var octaveNumber = (number + 5) / 7;
        var letterName = new MusicNote(number, chromaticSign).LetterName;
        if (octaveNumber <= 2)
        {
            return letterName.ToUpperInvariant();
        }

        var octaveSuffix = octaveNumber >= 4 ? "⁰¹²³⁴⁵⁶⁷⁸⁹"[octaveNumber - 3].ToString() : string.Empty;
        return letterName + octaveSuffix;
    }

    public static int GetLetterLowLineCount(string note)
    {
        var number = NoteNumber(note);
        if (number < 0)
        {
            return 0;
        }

        var octaveNumber = (number + 5) / 7;
        return octaveNumber switch
        {
            0 => 2,
            1 => 1,
            _ => 0
        };
    }

    private static string ToLetterFlatName(string symbolicName)
    {
        if (string.IsNullOrEmpty(symbolicName))
        {
            return symbolicName;
        }

        var baseLetter = symbolicName[0];
        var suffix = symbolicName[1..];
        var mappedFlat = char.ToLowerInvariant(baseLetter) switch
        {
            'c' => "ces",
            'd' => "des",
            'e' => "es",
            'f' => "fes",
            'g' => "ges",
            'a' => "as",
            'h' => "b",
            _ => symbolicName
        };

        if (mappedFlat == symbolicName)
        {
            return symbolicName;
        }

        if (char.IsUpper(baseLetter))
        {
            mappedFlat = mappedFlat.ToUpperInvariant();
        }

        return mappedFlat + suffix;
    }

    private static IReadOnlyList<string> CreateNoteList(int firstNumber, int lastNumber)
    {
        var notes = new string[lastNumber - firstNumber + 1];
        for (var number = firstNumber; number <= lastNumber; number++)
        {
            notes[number - firstNumber] = FormatNote(number);
        }

        return notes;
    }

    private static string FormatNote(int number)
    {
        var octaveNumber = (number + 5) / 7;
        var noteIndex = (number + 5) % 7;
        return $"{NoteNames[noteIndex]} {OctaveNames[octaveNumber]}";
    }
}
