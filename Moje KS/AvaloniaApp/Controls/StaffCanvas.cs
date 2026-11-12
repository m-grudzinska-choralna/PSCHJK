using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting;
using Avalonia.Platform;
using AvaloniaApp.Models;

namespace AvaloniaApp.Controls;

public sealed class StaffCanvas : Control
{
    private static readonly Bitmap TrebleClefImage = LoadImage("klucz__g.png");
    private static readonly Bitmap BassClefImage = LoadImage("klucz_f.png");
    private static readonly Bitmap WholeNoteImage = LoadImage("cala_nuta.png");

    public static readonly StyledProperty<string> ClefProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Clef), "wiolinowy");

    public static readonly StyledProperty<string> NoteProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Note), "C4");

    public static readonly StyledProperty<string> AnswerProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Answer), "C4");

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
        AffectsRender<StaffCanvas>(ClefProperty, NoteProperty, AnswerProperty, ShowAnswersProperty, UseLetterNamesProperty);
        ClipToBounds = true;
        MinHeight = 260;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var bounds = Bounds;
        var isBassClef = Clef == "basowy";
        var middleLineNoteNumber = isBassClef ? 17 : 29;
        var noteNumber = NoteRangeSettings.NoteNumber(Note);
        if (noteNumber < 0)
        {
            noteNumber = NoteRangeSettings.NoteNumber("C4");
        }

        var supportedNotes = isBassClef ? NoteRangeSettings.BassNotes : NoteRangeSettings.TrebleNotes;
        var lowestNoteNumber = NoteRangeSettings.NoteNumber(supportedNotes[0]);
        var highestNoteNumber = NoteRangeSettings.NoteNumber(supportedNotes[^1]);
        var lowestNoteStep = (lowestNoteNumber - middleLineNoteNumber) / 2d;
        var highestNoteStep = (highestNoteNumber - middleLineNoteNumber) / 2d;
        var clefHeightFactor = isBassClef ? 3.05 : 8;
        var clefLineOffset = isBassClef ? 5 : 7.3;
        var clefTopExtent = clefLineOffset - 3;
        var clefBottomExtent = 3 - clefLineOffset + clefHeightFactor;
        var topExtent = Math.Max(2, Math.Max(clefTopExtent, highestNoteStep + 0.55)) + 0.5;
        var bottomExtent = Math.Max(2, Math.Max(clefBottomExtent, -lowestNoteStep + 0.55)) + 0.5;
        var spacing = Math.Max(6, Math.Min((bounds.Width - 40) / 18, bounds.Height / (topExtent + bottomExtent)));
        var centerY = topExtent * spacing + Math.Max(0, bounds.Height - (topExtent + bottomExtent) * spacing) / 2;
        var left = spacing * 0.2;
        var right = Math.Max(left + spacing * 10, bounds.Width - spacing * 0.2);
        var pen = new Pen(Brushes.Black, Math.Max(1, spacing * 0.09));

        for (var line = 0; line < 5; line++)
        {
            var y = centerY - (2 * spacing) + (line * spacing);
            context.DrawLine(pen, new Point(left, y), new Point(right, y));
        }

        var clefImage = isBassClef ? BassClefImage : TrebleClefImage;
        var clefHeight = spacing * clefHeightFactor;
        var clefWidth = clefImage.Size.Width / clefImage.Size.Height * clefHeight;
        DrawImage(context, clefImage, new Rect(left, centerY + (3 - clefLineOffset) * spacing, clefWidth, clefHeight));

        var notes = new[] { Note };
        var notePen = new Pen(Brushes.Black, Math.Max(1, spacing * 0.12));
        for (var index = 0; index < notes.Length; index++)
        {
            var x = left + spacing * 6.3 + (index * spacing * 5.5);
            if (x > right - spacing)
            {
                break;
            }

            var y = centerY - GetStaffOffset(noteNumber, Clef, spacing);
            DrawLedgerLines(context, x, noteNumber, Clef, centerY, spacing, notePen);
            var noteHeight = spacing * 5.08;
            var noteWidth = WholeNoteImage.Size.Width / WholeNoteImage.Size.Height * noteHeight;
            DrawImage(context, WholeNoteImage, new Rect(x - noteWidth / 2, y - spacing * 4.44, noteWidth, noteHeight));

            if (ShowAnswers && !isBassClef)
            {
                var nameMode = UseLetterNames ? NoteNameMode.Letter : NoteNameMode.Solmization;
                var label = new FormattedText(NoteRangeSettings.ToSymbolicName(Answer, nameMode), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), spacing, Brushes.Black);
                context.DrawText(label, new Point(x - spacing, centerY + spacing * 6.5));
            }
        }
    }

    private static Bitmap LoadImage(string fileName)
    {
        using var stream = AssetLoader.Open(new Uri($"avares://AvaloniaApp/Assets/{fileName}"));
        return new Bitmap(stream);
    }

    private static void DrawImage(DrawingContext context, Bitmap image, Rect destination)
    {
        context.DrawImage(image, new Rect(image.Size), destination);
    }

    private static double GetStaffOffset(int noteNumber, string clef, double spacing)
    {
        var middleLineNoteNumber = clef == "basowy" ? 17 : 29;
        return (noteNumber - middleLineNoteNumber) * (spacing / 2);
    }

    private static void DrawLedgerLines(
        DrawingContext context,
        double x,
        int noteNumber,
        string clef,
        double centerY,
        double spacing,
        Pen pen)
    {
        var bottomLineNoteNumber = clef == "basowy" ? 13 : 25;
        var topLineNoteNumber = clef == "basowy" ? 21 : 33;

        for (var ledgerNoteNumber = bottomLineNoteNumber - 2;
             ledgerNoteNumber >= noteNumber;
             ledgerNoteNumber -= 2)
        {
            var y = centerY - GetStaffOffset(ledgerNoteNumber, clef, spacing);
            context.DrawLine(pen, new Point(x - spacing * 1.3, y), new Point(x + spacing * 1.3, y));
        }

        for (var ledgerNoteNumber = topLineNoteNumber + 2;
             ledgerNoteNumber <= noteNumber;
             ledgerNoteNumber += 2)
        {
            var y = centerY - GetStaffOffset(ledgerNoteNumber, clef, spacing);
            context.DrawLine(pen, new Point(x - spacing * 1.3, y), new Point(x + spacing * 1.3, y));
        }
    }
}
