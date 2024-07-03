namespace apief
{
    public class NoteRepository : INoteRepository
    {
        DataContextEF _entityFramework;
        // IUserRepository _userRepository;
        public NoteRepository(DataContextEF entityFramework)
        {
            _entityFramework = entityFramework;
        }
        public Note AddNote(int userId, NoteDto noteDto)
        {
            var note = new Note
            {
                Id = userId,
                Title = noteDto.Title,
                Description = noteDto.Description,
                Done = noteDto.Done
            };
            _entityFramework.Add(note);
            _entityFramework.SaveChanges();
            return note;
        }

        public void UpdateData(int noteId, int userId, NoteDto updatedNoteDto)
        {
            var noteToUpdate = _entityFramework.Notes.FirstOrDefault(e => e.NoteId == noteId && e.Id == userId);

            if (noteToUpdate != null)
            {
                // Обновляем свойства записи
                noteToUpdate.Title = updatedNoteDto.Title;
                noteToUpdate.Description = updatedNoteDto.Description;
                noteToUpdate.Done = updatedNoteDto.Done;
                _entityFramework.SaveChanges();
            }
            else
            {
                throw new Exception("Failed to update Note");
            }


        }

        public Note GetNote(int noteId, int userId)
        {
            var expense = _entityFramework.Notes.FirstOrDefault(e => e.NoteId == noteId && e.Id == userId);

            if (expense == null)
            {
                throw new Exception("Note Not Found");
            }
            else
            {
                return expense;
            }


        }

        public List<Note> GetNotes(int userId)
        {
            IEnumerable<Note> notes = _entityFramework.Notes.Where(e => e.Id == userId).ToList();


            List<Note> notesresult = notes.ToList();
            return notesresult;
            // throw new Exception("Data Is Empty");

        }

        public void DeleteNote(int noteId, int userId)
        {
            var note = _entityFramework.Notes.FirstOrDefault(e => e.NoteId == noteId && e.Id == userId);

            if (note != null)
            {
                _entityFramework.Notes.Remove(note);
                _entityFramework.SaveChanges(); // Сохраняем изменения в базе данных

            }
            else
            {
                throw new Exception("Note Not Found");
            }
        }


    }
}