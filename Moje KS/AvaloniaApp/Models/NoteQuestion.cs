namespace AvaloniaApp.Models;

public sealed record NoteQuestion(string Note, string Clef, string Answer, string Accidental)
{
    public bool IsAnswerVisible { get; set; }
}
