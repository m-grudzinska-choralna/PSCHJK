namespace KsztalcenieSluchu.Models;

public sealed class NoteQuestion
{
    public NoteQuestion(MusicNote noteData, string clef, Ks2Screen? ks2Screen = null)
    {
        NoteData = noteData;
        Clef = clef;
        Note = NoteRangeSettings.Notes[noteData.Number];
        Answer = NoteRangeSettings.Notes[noteData.Number];
        Accidental = noteData.ChromaticSign;
        Ks2Screen = ks2Screen ?? CreateDefaultScreen(noteData);
    }

    public NoteQuestion(string note, string clef, string answer, string accidental)
    {
        NoteData = MusicNote.FromDisplayName(note, accidental);
        Note = note;
        Clef = clef;
        Answer = answer;
        Accidental = accidental;
        Ks2Screen = CreateDefaultScreen(NoteData);
    }

    public MusicNote NoteData { get; }
    public Ks2Screen Ks2Screen { get; }
    public Ks2RecognizeNoteScreenContent Ks2Content => (Ks2RecognizeNoteScreenContent)Ks2Screen.Content;
    public string Note { get; }
    public string Clef { get; }
    public string Answer { get; }
    public string Accidental { get; }
    public bool IsAnswerVisible { get; set; }

    private static Ks2Screen CreateDefaultScreen(MusicNote noteData)
    {
        var element = new Ks2Element<MusicNote>("nuta", 0, noteData, noteData);
        return new Ks2Screen("rozpoznawanie", "nuty", element);
    }
}
