using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace apief;

[Authorize]
[ApiController]
[Route("api[controller]")]
public class NoteController : ControllerBase
{
     private readonly INoteService _noteService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public IConfiguration Config { get; }
    public NoteController NotesService { get; }

    public NoteController(INoteService noteService, IMapper mapper, IConfiguration config )
    {
        _noteService = noteService;
        _mapper = mapper;
        _config = config;
    }



    [HttpPost("PostNote")]
    public IActionResult PostNote(NoteDto noteDto)
    {
        try
        {
            int userId = GetUserId();
            
            NoteDto mappedNoteDto = _mapper.Map<NoteDto>(noteDto);
            Note note = _noteService.AddNote(userId, mappedNoteDto); // Используем метод AddNote из NoteService
            return Ok(note);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("UpdateNote/{noteId}")]
    public IActionResult UpdateNote(int noteId , NoteDto updatedNoteDto)
    {
        try
        {
            int userId = GetUserId();
            _noteService.UpdateNote(userId, noteId, updatedNoteDto); // Используем метод UpdateNote из NoteService
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetNote/{noteId}")]
    public IActionResult GetSingleNote(int noteId)
    {
        try
        {
            int userId = GetUserId();
            Note note = _noteService.GetNoteById(userId, noteId); // Используем метод GetNoteById из NoteService
            return Ok(note);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetNotes")]
    public IActionResult GetNotes()
    {
        try
        {
            int userId = GetUserId();
            List<Note> notes = _noteService.GetNotesByUserId(userId); // Используем метод GetNotesByUserId из NoteService
            return Ok(notes);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteNote/{noteId}")]
    public IActionResult DeleteNote(int noteId )
    {
        try
        {
            int userId = GetUserId();
            _noteService.DeleteNoteById(userId, noteId); // Используем метод DeleteNoteById из NoteService
            return Ok("Note Was Deleted");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    // поместить в репозиторий

    [NonAction]
    protected virtual int GetUserId()
    {
        string? accessToken = HttpContext.Request.Headers["Authorization"];
        if (accessToken != null && accessToken.StartsWith("Bearer "))
        {
            accessToken = accessToken.Substring("Bearer ".Length);
        }
        accessToken = accessToken?.Trim();
        int userId = 0;

        using (var dbContext = new DataContextEF(_config))
        {
            var token = dbContext.Accounts.FirstOrDefault(t => t.TokenValue == accessToken);
            if (token != null)
            {
                userId = token.Id;
                return userId;
            }
        }

        throw new Exception("Can't get user id");
    }

    private ObjectResult? checkAuthToken()
    {
        try
        {
            GetUserId();
        }
        catch (Exception ex)
        {
            return StatusCode(401, ex.Message);
        }
        return null;
    }
}
