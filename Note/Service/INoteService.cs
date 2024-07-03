namespace apief
{
    public interface INoteService
    {
        public Note AddNote(int userId, NoteDto noteDto);
        public void UpdateNote(int userId, int noteId, NoteDto noteDto);
        public Note GetNoteById(int userId, int noteId);
        public List<Note> GetNotesByUserId(int userId);
        public void DeleteNoteById(int userId, int noteId);
    }
}