using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvaloniaApp.Models;

namespace AvaloniaApp.Controls;

/// <summary>
/// Rysuje pięciolinię z jedną nutą według algorytmu KS2 (Rysowanie.RysujNute, RysujSymbol, RysujPodpisNuty).
/// Numer linii: linia 1 = dolna linia pięciolinii, linia 5 = górna.
/// </summary>
public sealed class StaffCanvas : Control
{
    private const double SignatureGapAfterAccidental = 0.3;
    private const double SignatureFontFactor = 1.1;
    private const double PointsToPixels = 96d / 72d;

    private static readonly Dictionary<string, Bitmap> Images = new();

    private sealed record ImageInfo(string File, double Factor, double Offset);

    private static readonly Dictionary<string, ImageInfo> ImageInfos = new()
    {
        ["klucz_g"] = new("klucz__g", 8, 7.3),
        ["klucz_f"] = new("klucz_f", 3.05, 5),
        ["cala_nuta"] = new("cala_nuta", 5.08, 4.44),
        ["polnuta_w_gore"] = new("polnuta_w_gore", 3.7, 3.2),
        ["cwiercnuta_w_gore"] = new("cwiercnuta_w_gore", 3.7, 3.2),
        ["osemka_w_gore"] = new("osemka_w_gore", 3.7, 3.2),
        ["szesnastka_w_gore"] = new("szesnastka_w_gore", 3.7, 3.2),
        ["polnuta_w_dol"] = new("polnuta_w_dol", 3.7, 0.5),
        ["cwiercnuta_w_dol"] = new("cwiercnuta_w_dol", 3.7, 0.5),
        ["osemka_w_dol"] = new("osemka_w_dol", 3.7, 0.5),
        ["szesnastka_w_dol"] = new("szesnastka_w_dol", 3.7, 0.5),
        ["bemol"] = new("bemol", 2.4, 1.8),
        ["krzyzyk"] = new("krzyzyk", 2.4, 1.2),
        ["podwojny_bemol"] = new("podwojny_bemol", 2.4, 1.8),
        ["podwojny_krzyzyk"] = new("podwojny_krzyzyk", 1, 0.5),
        ["kasownik"] = new("kasownik", 2.4, 1.2)
    };

    public static readonly StyledProperty<string> ClefProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Clef), "wiolinowy");

    public static readonly StyledProperty<string> NoteProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Note), "C4");

    public static readonly StyledProperty<string> AnswerProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Answer), "C4");

    public static readonly StyledProperty<string> AccidentalProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Accidental), "none");

    /// <summary>Znak chromatyczny obowiązujący dla tej nuty przed jej narysowaniem (nazwy KS2: brak, bemol, krzyzyk, podwojny_bemol, podwojny_krzyzyk).</summary>
    public static readonly StyledProperty<string> PreviousAccidentalProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(PreviousAccidental), "brak");

    public static readonly StyledProperty<string> RhythmProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Rhythm), "cala_nuta");

    public static readonly StyledProperty<bool> ShowAnswersProperty =
        AvaloniaProperty.Register<StaffCanvas, bool>(nameof(ShowAnswers));

    public static readonly StyledProperty<bool> UseLetterNamesProperty =
        AvaloniaProperty.Register<StaffCanvas, bool>(nameof(UseLetterNames), true);

    public string Clef
    {
        get => GetValue(ClefProperty);
        set => SetValue(ClefProperty, value);
    }

    public string Note
    {
        get => GetValue(NoteProperty);
        set => SetValue(NoteProperty, value);
    }

    public string Answer
    {
        get => GetValue(AnswerProperty);
        set => SetValue(AnswerProperty, value);
    }

    public string Accidental
    {
        get => GetValue(AccidentalProperty);
        set => SetValue(AccidentalProperty, value);
    }

    public string PreviousAccidental
    {
        get => GetValue(PreviousAccidentalProperty);
        set => SetValue(PreviousAccidentalProperty, value);
    }

    public string Rhythm
    {
        get => GetValue(RhythmProperty);
        set => SetValue(RhythmProperty, value);
    }

    public bool ShowAnswers
    {
        get => GetValue(ShowAnswersProperty);
        set => SetValue(ShowAnswersProperty, value);
    }

    public bool UseLetterNames
    {
        get => GetValue(UseLetterNamesProperty);
        set => SetValue(UseLetterNamesProperty, value);
    }

    public StaffCanvas()
    {
        AffectsRender<StaffCanvas>(
            ClefProperty,
            NoteProperty,
            AnswerProperty,
            AccidentalProperty,
            PreviousAccidentalProperty,
            RhythmProperty,
            ShowAnswersProperty,
            UseLetterNamesProperty);
        ClipToBounds = true;
        MinHeight = 260;
    }

    // KS2 Nuta.NumerLinii
    private static double LineNumber(int noteNumber, bool bass) =>
        bass ? (noteNumber - 23) / 2d + 6 : (noteNumber - 23) / 2d;

    // KS2 Nuta.NumerLiniiEfektywny
    private static double EffectiveLineNumber(int noteNumber, bool bass, out int transfer)
    {
        var line = LineNumber(noteNumber, bass);
        transfer = 0;
        if (!bass && line >= 9.5)
        {
            line -= 3.5;
            transfer = 1;
        }

        if (bass && line <= -5)
        {
            line += 3.5;
            transfer = -1;
        }

        return line;
    }

    private static string ToKs2Sign(string accidental) => accidental switch
    {
        "flat" or "bemol" => "bemol",
        "sharp" or "krzyzyk" => "krzyzyk",
        "double-flat" or "podwojny_bemol" => "podwojny_bemol",
        "double-sharp" or "podwojny_krzyzyk" => "podwojny_krzyzyk",
        _ => "brak"
    };

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var bounds = Bounds;
        var isBassClef = Clef == "basowy";
        var noteNumber = NoteRangeSettings.NoteNumber(Note);
        if (noteNumber < 0)
        {
            noteNumber = NoteRangeSettings.NoteNumber("C4");
        }

        var supportedNotes = isBassClef ? NoteRangeSettings.BassNotes : NoteRangeSettings.TrebleNotes;
        var lowestNumber = NoteRangeSettings.NoteNumber(supportedNotes[0]);
        var highestNumber = NoteRangeSettings.NoteNumber(supportedNotes[^1]);

        var topLine = 5d;
        var bottomLine = 1d;
        var topTransfer = false;
        var bottomTransfer = false;
        for (var number = lowestNumber; number <= highestNumber; number++)
        {
            var supportedLine = EffectiveLineNumber(number, isBassClef, out var supportedTransfer);
            topTransfer |= supportedTransfer == 1;
            bottomTransfer |= supportedTransfer == -1;
            topLine = Math.Max(topLine, supportedLine + (supportedLine < 3 ? 3.2 : 0.5));
            bottomLine = Math.Min(bottomLine, supportedLine < 3 ? supportedLine - 0.5 : supportedLine - 3.2);
        }

        if (topTransfer)
        {
            topLine = Math.Max(topLine, 9.5 + 1.2);
        }

        if (bottomTransfer)
        {
            bottomLine = Math.Min(bottomLine, -4.5 - 1.3);
        }

        var clefInfo = ImageInfos[isBassClef ? "klucz_f" : "klucz_g"];
        topLine = Math.Max(topLine, clefInfo.Offset);
        bottomLine = Math.Min(bottomLine, clefInfo.Offset - clefInfo.Factor);

        var topExtent = topLine - 3 + 0.3;
        var bottomExtent = 3 - bottomLine + 0.3;
        var spacing = Math.Max(6, Math.Min((bounds.Width - 40) / 18, bounds.Height / (topExtent + bottomExtent)));
        var centerY = topExtent * spacing + Math.Max(0, bounds.Height - (topExtent + bottomExtent) * spacing) / 2;
        var left = spacing * 0.2;
        var right = Math.Max(left + spacing * 10, bounds.Width - spacing * 0.2);
        var linePen = new Pen(Brushes.Black, Math.Max(1, spacing * 0.09));

        double LineY(double line) => centerY - (line - 3) * spacing;

        for (var line = 1; line <= 5; line++)
        {
            context.DrawLine(linePen, new Point(left, LineY(line)), new Point(right, LineY(line)));
        }

        DrawSymbol(context, clefInfo, left, LineY(clefInfo.Offset), spacing, out _);

        var position = left + spacing * 6;
        var numberLine = EffectiveLineNumber(noteNumber, isBassClef, out var transfer);

        var ks2Sign = ToKs2Sign(Accidental);
        var effectiveSign = Ks2AccidentalState.Effective(ks2Sign, ToKs2Sign(PreviousAccidental));

        var signWidth = 0d;
        var gapAfterSign = 0d;
        if (effectiveSign != "brak")
        {
            DrawSymbol(context, ImageInfos[effectiveSign], position, LineY(numberLine + ImageInfos[effectiveSign].Offset), spacing, out signWidth);
            gapAfterSign = SignatureGapAfterAccidental * spacing;
        }

        var noteKey = Rhythm == "cala_nuta"
            ? "cala_nuta"
            : Rhythm + (numberLine < 3 ? "_w_gore" : "_w_dol");
        var noteInfo = ImageInfos[noteKey];
        var noteX = position + signWidth + gapAfterSign;
        DrawSymbol(context, noteInfo, noteX, LineY(numberLine + noteInfo.Offset), spacing, out var noteWidth);
        var totalWidth = signWidth + gapAfterSign + noteWidth;

        if (numberLine > 5 || numberLine <= 0)
        {
            int first;
            int last;
            var maxLine = (int)(numberLine * 2) / 2;
            if (numberLine > 5)
            {
                first = 6;
                last = maxLine;
            }
            else
            {
                first = maxLine;
                last = 0;
            }

            var extensionLeft = SignatureGapAfterAccidental * spacing;
            var extensionRight = extensionLeft;
            if (signWidth > 0)
            {
                extensionLeft = 0;
            }

            for (var line = first; line <= last; line++)
            {
                context.DrawLine(
                    linePen,
                    new Point(position + signWidth - extensionLeft, LineY(line)),
                    new Point(position + totalWidth + extensionRight, LineY(line)));
            }
        }

        if (transfer != 0)
        {
            var x = position - 2 * spacing;
            var y = transfer == 1 ? LineY(9.5) : LineY(-4.5);
            var eight = new FormattedText("8", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), spacing * PointsToPixels, Brushes.Black);
            context.DrawText(eight, new Point(x, transfer == 1 ? y - 0.4 * spacing : y - 1.1 * spacing));

            var dotted = new Pen(Brushes.Black, Math.Max(1, spacing * 0.09), DashStyle.Dot);
            context.DrawLine(dotted, new Point(x + 1.3 * spacing, y), new Point(x + 4.8 * spacing, y));
        }
    }

    // KS2 Rysowanie.RysujSymbol
    private static void DrawSymbol(DrawingContext context, ImageInfo info, double x, double y, double spacing, out double width)
    {
        var image = GetImage(info.File);
        var height = spacing * info.Factor;
        width = image.Size.Width * height / image.Size.Height;
        context.DrawImage(image, new Rect(image.Size), new Rect(x, y, width, height));
    }

    // KS2 Rysowanie.RysujPodpisNuty
    private void DrawSignature(DrawingContext context, int noteNumber, double centerX, double y, double spacing)
    {
        var note = new MusicNote(noteNumber, Accidental, Rhythm);
        var octave = note.OctaveNumber;
        var heightPt = SignatureFontFactor * spacing;
        var fontSize = heightPt * PointsToPixels;
        var smallFontSize = fontSize * 0.65;
        var typeface = new Typeface("Courier New", FontStyle.Normal, FontWeight.Bold);

        string text1;
        var text2 = string.Empty;
        if (UseLetterNames)
        {
            text1 = octave > 2 ? note.LetterName : note.LetterName.ToUpperInvariant();
            if (octave >= 4)
            {
                text2 = (octave - 3).ToString(CultureInfo.InvariantCulture);
            }
        }
        else
        {
            text1 = note.SolmizationName;
        }

        var underline = octave <= 1;
        var first = new FormattedText(text1, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
        if (underline)
        {
            first.SetTextDecorations(TextDecorations.Underline);
        }

        var second = new FormattedText(text2, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, smallFontSize, Brushes.Black);
        var length1 = first.WidthIncludingTrailingWhitespace;
        var length2 = second.WidthIncludingTrailingWhitespace;
        var totalLength = length1 + length2 * 0.75;
        var startX = centerX - totalLength / 2;

        context.DrawText(first, new Point(startX, y));
        if (text2.Length > 0)
        {
            context.DrawText(second, new Point(startX + length1 + heightPt * 0.2, y));
        }

        if (octave == 0)
        {
            var spaces = new FormattedText(new string(' ', text1.Length), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
            spaces.SetTextDecorations(TextDecorations.Underline);
            context.DrawText(spaces, new Point(startX, y + heightPt / 4));
        }
    }

    private static Bitmap GetImage(string file)
    {
        if (!Images.TryGetValue(file, out var image))
        {
            using var stream = AssetLoader.Open(new Uri($"avares://AvaloniaApp/Assets/{file}.png"));
            image = new Bitmap(stream);
            Images[file] = image;
        }

        return image;
    }
}
