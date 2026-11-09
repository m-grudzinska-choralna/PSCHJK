using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using AvaloniaApp.Models;

namespace AvaloniaApp.Controls;

public sealed class StaffCanvas : Control
{
    public static readonly StyledProperty<string> ClefProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Clef), "wiolinowy");

    public static readonly StyledProperty<string> FromNoteProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(FromNote), "C4");

    public static readonly StyledProperty<string> ToNoteProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(ToNote), "C5");

    public static readonly StyledProperty<string> NoteProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Note), "C4");

    public static readonly StyledProperty<string> AnswerProperty =
        AvaloniaProperty.Register<StaffCanvas, string>(nameof(Answer), "C4");

    public static readonly StyledProperty<bool> ShowAnswersProperty =
        AvaloniaProperty.Register<StaffCanvas, bool>(nameof(ShowAnswers));

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

    public string FromNote
    {
        get => GetValue(FromNoteProperty);
        set => SetValue(FromNoteProperty, value);
    }

    public string ToNote
    {
        get => GetValue(ToNoteProperty);
        set => SetValue(ToNoteProperty, value);
    }

    public bool ShowAnswers
    {
        get => GetValue(ShowAnswersProperty);
        set => SetValue(ShowAnswersProperty, value);
    }

    public StaffCanvas()
    {
        AffectsRender<StaffCanvas>(ClefProperty, FromNoteProperty, ToNoteProperty, NoteProperty, AnswerProperty, ShowAnswersProperty);
        ClipToBounds = true;
        MinHeight = 260;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var bounds = Bounds;
        var centerY = bounds.Height / 2;
        var left = 54d;
        var right = Math.Max(left + 120, bounds.Width - 30);
        var spacing = 14d;
        var pen = new Pen(Brushes.Black, 1.5);

        for (var line = 0; line < 5; line++)
        {
            var y = centerY - (2 * spacing) + (line * spacing);
            context.DrawLine(pen, new Point(left, y), new Point(right, y));
        }

        var typeface = new Typeface("Segoe UI Symbol");
        var clefText = Clef == "basowy" ? "𝄢" : "𝄞";
        var glyph = new FormattedText(clefText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 64, Brushes.Black);
        context.DrawText(glyph, new Point(12, centerY - 43));

        var notes = new[] { Note };
        var notePen = new Pen(Brushes.Black, 2);
        for (var index = 0; index < notes.Length; index++)
        {
            var x = left + 75 + (index * 72);
            if (x > right - 20)
            {
                break;
            }

            var y = centerY - GetStaffOffset(notes[index], spacing);
            context.DrawEllipse(Brushes.Black, notePen, new Point(x, y), 9, 6);
            context.DrawLine(notePen, new Point(x + 8, y), new Point(x + 8, y - 48));

            if (ShowAnswers)
            {
                var label = new FormattedText(Answer, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 14, Brushes.Black);
                context.DrawText(label, new Point(x - 12, centerY + 92));
            }
        }
    }

    private static double GetStaffOffset(string note, double spacing)
    {
        var index = Array.IndexOf(NoteRangeSettings.Notes.ToArray(), note);
        return (index - 7) * (spacing / 2);
    }
}
