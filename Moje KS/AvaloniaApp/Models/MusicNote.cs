using System;
using System.Collections.Generic;

namespace AvaloniaApp.Models;

public sealed record NoteOctave(int Number, string Name, string NoteName);

public sealed record RhythmicValue(string Name, float ReciprocalLength, float Length)
{
    public static IReadOnlyDictionary<string, RhythmicValue> Values { get; } =
        new Dictionary<string, RhythmicValue>(StringComparer.Ordinal)
        {
            ["cala_nuta"] = new("cala_nuta", 1, 1),
            ["polnuta"] = new("polnuta", 2, 0.5f),
            ["cwiercnuta"] = new("cwiercnuta", 4, 0.25f),
            ["osemka"] = new("osemka", 8, 0.125f),
            ["szesnastka"] = new("szesnastka", 16, 1 / 16.0f)
        };

    public static RhythmicValue Get(string name)
    {
        if (Values.TryGetValue(name, out var value))
        {
            return value;
        }

        throw new ArgumentException("Unknown rhythmic value.", nameof(name));
    }
}

public sealed class MusicNote
{
    private static readonly string[] LetterNames = { "c", "d", "e", "f", "g", "a", "h" };
    private static readonly string[] FlatLetterNames = { "ces", "des", "es", "fes", "ges", "as", "b" };
    private static readonly string[] DoubleFlatLetterNames = { "ceses", "deses", "eses", "feses", "geses", "asas", "heses" };
    private static readonly string[] SharpLetterNames = { "cis", "dis", "eis", "fis", "gis", "ais", "his" };
    private static readonly string[] DoubleSharpLetterNames = { "cisis", "disis", "eisis", "fisis", "gisis", "aisis", "hisis" };
    private static readonly string[] SolmizationNames = { "do", "re", "mi", "fa", "sol", "la", "si" };

    public static IReadOnlyList<NoteOctave> Octaves { get; } =
        new[]
        {
            new NoteOctave(0, "subkontra", "subkontra"),
            new NoteOctave(1, "kontra", "kontra"),
            new NoteOctave(2, "wielka", "wielkie"),
            new NoteOctave(3, "mała", "małe"),
            new NoteOctave(4, "razkreślna", "razkreślne"),
            new NoteOctave(5, "dwukreślna", "dwukreślne"),
            new NoteOctave(6, "trzykreślna", "trzykreślne"),
            new NoteOctave(7, "czterokreślna", "czterokreślne"),
            new NoteOctave(8, "pięciokreślna", "pięciokreślne")
        };

    public int Number { get; }
    public string FullName { get; }
    public string BaseLetterName { get; }
    public string LetterName { get; }
    public string SolmizationName { get; }
    public int OctaveNumber { get; }
    public NoteOctave Octave { get; }
    public string ChromaticSign { get; }
    public string Ks2ChromaticSign { get; }
    public RhythmicValue RhythmicValue { get; }
    public int NumberInOctave { get; }
    public int DistanceToNext { get; }
    public int DistanceToPrevious { get; }
    public float SignatureSize { get; set; } = 1.1f;
    public string SignatureType { get; set; } = "brak";
    public string? OtherSignature { get; set; }

    public MusicNote(int number, string chromaticSign = "none", string rhythmicValueName = "cala_nuta")
    {
        if (number < 0 || number >= NoteRangeSettings.Notes.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(number));
        }

        Number = number;
        ChromaticSign = NormalizeChromaticSign(chromaticSign);
        Ks2ChromaticSign = ToKs2ChromaticSign(ChromaticSign);
        RhythmicValue = RhythmicValue.Get(rhythmicValueName);

        OctaveNumber = (Number + 5) / 7;
        Octave = Octaves[OctaveNumber];
        NumberInOctave = (Number + 5) % 7;

        var letters = GetLetterSet(Ks2ChromaticSign);
        LetterName = letters[NumberInOctave];
        BaseLetterName = LetterNames[NumberInOctave];
        SolmizationName = SolmizationNames[NumberInOctave];
        FullName = $"{LetterName} {Octave.NoteName}";

        DistanceToNext = NumberInOctave is 2 or 6 ? 1 : 2;
        DistanceToPrevious = NumberInOctave is 0 or 3 ? 1 : 2;
    }

    public static MusicNote FromDisplayName(string note, string chromaticSign = "none", string rhythmicValueName = "cala_nuta")
    {
        var number = NoteRangeSettings.NoteNumber(note);
        if (number < 0)
        {
            throw new ArgumentException("Unknown note.", nameof(note));
        }

        return new MusicNote(number, chromaticSign, rhythmicValueName);
    }

    private static string[] GetLetterSet(string ks2ChromaticSign)
    {
        return ks2ChromaticSign switch
        {
            "krzyzyk" => SharpLetterNames,
            "bemol" => FlatLetterNames,
            "podwojny_krzyzyk" => DoubleSharpLetterNames,
            "podwojny_bemol" => DoubleFlatLetterNames,
            _ => LetterNames
        };
    }

    private static string NormalizeChromaticSign(string chromaticSign)
    {
        return chromaticSign switch
        {
            "none" or "flat" or "sharp" or "double-flat" or "double-sharp" => chromaticSign,
            "brak" => "none",
            "bemol" => "flat",
            "krzyzyk" => "sharp",
            "podwojny_bemol" => "double-flat",
            "podwojny_krzyzyk" => "double-sharp",
            _ => throw new ArgumentException("Unknown chromatic sign.", nameof(chromaticSign))
        };
    }

    private static string ToKs2ChromaticSign(string chromaticSign)
    {
        return chromaticSign switch
        {
            "flat" => "bemol",
            "sharp" => "krzyzyk",
            "double-flat" => "podwojny_bemol",
            "double-sharp" => "podwojny_krzyzyk",
            _ => "brak"
        };
    }
}