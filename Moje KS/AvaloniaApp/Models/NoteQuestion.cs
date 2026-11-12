namespace AvaloniaApp.Models;

public sealed class NoteQuestion
{
    public NoteQuestion(MusicNote noteData, string clef)
    {
        NoteData = noteData;
        Clef = clef;
        Note = NoteRangeSettings.Notes[noteData.Number];
        Answer = NoteRangeSettings.Notes[noteData.Number];
        Accidental = noteData.ChromaticSign;
    }

    public NoteQuestion(string note, string clef, string answer, string accidental)
    {
        NoteData = MusicNote.FromDisplayName(note, accidental);
        Note = note;
        Clef = clef;
        Answer = answer;
        Accidental = accidental;
    }

    public MusicNote NoteData { get; }
    public string Note { get; }
    public string Clef { get; }
    public string Answer { get; }
    public string Accidental { get; }
    public bool IsAnswerVisible { get; set; }
}
