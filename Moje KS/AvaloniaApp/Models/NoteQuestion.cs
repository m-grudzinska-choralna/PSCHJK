namespace AvaloniaApp.Models;

public sealed record NoteQuestion(string Note, string Clef, string Answer)
{
    public bool IsAnswerVisible { get; set; }
}
