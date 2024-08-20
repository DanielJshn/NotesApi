namespace apief
{
    public class NoteRepository : INoteRepository
    {
        DataContextEF _entityFramework;

        public NoteRepository(DataContextEF entityFramework)
        {
            _entityFramework = entityFramework;
        }

        public Note AddNote(int userId, NoteDto noteDto)
        {

            var note = new Note
            {

                Id = userId,
                CategoryId = noteDto.CategoryId,
                Title = noteDto.Title,
                Description = noteDto.Description,
                Done = noteDto.Done
            };

            _entityFramework.Add(note);
            _entityFramework.SaveChanges();
            return note;

        }
        public Category AddCategory(int userId, CategoryDto categoryDto)
        {

            var category = new Category
            {

                Id = userId,

                Name = categoryDto.Name
            };
            _entityFramework.Add(category);
            _entityFramework.SaveChanges();

            return category;
        }

        public void DeleteCategoryById(int categoryId, int userId)
        {
            var category = _entityFramework.Categories.FirstOrDefault(e => e.CategoryId == categoryId && e.Id == userId);
            _entityFramework.Categories.Remove(category);
            _entityFramework.SaveChanges();

        }

        public void UpdateData(int noteId, int userId, NoteDto updatedNoteDto)
        {
            var noteToUpdate = _entityFramework.Notes.FirstOrDefault(e => e.NoteId == noteId && e.Id == userId);

            noteToUpdate.Title = updatedNoteDto.Title;
            noteToUpdate.Description = updatedNoteDto.Description;
            noteToUpdate.Done = updatedNoteDto.Done;
            _entityFramework.SaveChanges();

        }

        public Note GetNote(int noteId, int userId)
        {
            var expense = _entityFramework.Notes.FirstOrDefault(e => e.NoteId == noteId && e.Id == userId);

            return expense;

        }

        public List<Note> GetNotes(int userId)
        {
            IEnumerable<Note> notes = _entityFramework.Notes.Where(e => e.Id == userId).ToList();

            List<Note> notesresult = notes.ToList();
            return notesresult;
        }

        public void DeleteNote(int noteId, int userId)
        {
            var note = _entityFramework.Notes.FirstOrDefault(e => e.NoteId == noteId && e.Id == userId);

            _entityFramework.Notes.Remove(note);
            _entityFramework.SaveChanges();

        }
    }
}