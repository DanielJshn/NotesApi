using apief;
using Microsoft.IdentityModel.Tokens;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;

    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public Note AddNote(int userId, NoteDto noteDto)
    {
        if (userId == 0)
        {
            throw new Exception("User not found!");
        }
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
        if (userId == 0)
        {
            throw new Exception("User not found!");
        }
        if (updatedNoteDto.Title.IsNullOrEmpty())
        {
            throw new Exception("Title is empty!");
        }
        if (updatedNoteDto.Description.IsNullOrEmpty())
        {
            throw new Exception("Description is empty!");
        }
        _noteRepository.UpdateData(noteId, userId, updatedNoteDto);
    }

    public List<Note> GetNotesByUserId(int userId)
    {
        if (userId == 0)
        {
            throw new Exception("User not found!");
        }
        return _noteRepository.GetNotes(userId);
    }

    public void DeleteNoteById(int userId, int noteId)
    {
        if (userId == 0)
        {
            throw new Exception("User not found!");
        }
        _noteRepository.DeleteNote(noteId, userId);
    }

    public Category AddCategory(int userId, CategoryDto categoryDto)
    {
        if (userId == 0)
        {
            throw new Exception("User not found!");
        }
        return _noteRepository.AddCategory(userId, categoryDto);
    }

    public void DeleteCategoryById(int categoryId, int userId)
    {
        if (userId == 0)
        {
            throw new Exception("User not found!");
        }
         _noteRepository.DeleteCategoryById(categoryId, userId);
    }

}
