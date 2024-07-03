using apief;
using Microsoft.IdentityModel.Tokens;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;

    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }
    private Note MapToNoteObject(int userId, NoteDto noteDto)
    {
        // Преобразование данных из NoteDto в Note
        return new Note
        {
            Id = userId,
            Title = noteDto.Title,
            Description = noteDto.Description,
            // Другие поля, которые необходимо скопировать из noteDto в Note
        };
    }
    public Note AddNote(int userId, NoteDto noteDto)
    {
        if (noteDto == null)
        {
            throw new Exception("Note data is null!");
        }
        if (noteDto.Title.IsNullOrEmpty())
        {
            throw new Exception("Title is empty!");
        }
        if (noteDto.Description.IsNullOrEmpty())
        {
            throw new Exception("Description is empty!");
        }
        return _noteRepository.AddNote(userId, noteDto);
    }

    public void UpdateNote(int userId, int noteId, NoteDto updatedNoteDto)
    {
        // Ваша бизнес-логика для обновления заметки
        _noteRepository.UpdateData(noteId, userId, updatedNoteDto);
    }

    public Note GetNoteById(int userId, int noteId)
    {
        // Ваша бизнес-логика для получения заметки по идентификатору
        return _noteRepository.GetNote(noteId, userId);
    }

    public List<Note> GetNotesByUserId(int userId)
    {
        // Ваша бизнес-логика для получения всех заметок пользователя
        return _noteRepository.GetNotes(userId);
    }

    public void DeleteNoteById(int userId, int noteId)
    {
        // Ваша бизнес-логика для удаления заметки по идентификатору
        _noteRepository.DeleteNote(noteId, userId);
    }

    // Другие методы и бизнес-логика, если необходимо
}
