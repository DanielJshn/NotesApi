namespace apief
{
    public interface INoteRepository
    {
        public Note AddNote(int userId, NoteDto note);
        public void UpdateData(int noteId, int userId, NoteDto noteDto);
        public Note GetNote(int noteId, int userId);
        public List<Note> GetNotes(int userId);
        public void DeleteNote(int noteId, int userId);
    }
}