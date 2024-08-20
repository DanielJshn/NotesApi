using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace apief;

[Authorize]
[ApiController]
[Route("note[controller]")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly TokenOpperation _tokenOpperation;
    

    public NoteController(INoteService noteService, IMapper mapper, IConfiguration config, TokenOpperation tokenOpperation)
    {
        _noteService = noteService;
        _mapper = mapper;
        _config = config;
        _tokenOpperation = tokenOpperation;
    }
    
    


    [HttpPost("PostNote")]

    public IActionResult PostNote(NoteDto noteDto)
    {

        try
        {
            int userId = _tokenOpperation.GetUserIdFromToken();
            NoteDto mappedNoteDto = _mapper.Map<NoteDto>(noteDto);
            Note note = _noteService.AddNote(userId, mappedNoteDto);
            return Ok(note);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }


    [HttpPut("UpdateNote/{noteId}")]
    public IActionResult UpdateNote(int noteId, NoteDto updatedNoteDto)
    {

        try
        {
            int userId = _tokenOpperation.GetUserIdFromToken();
            _noteService.UpdateNote(userId, noteId, updatedNoteDto);
            return Ok();
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
            int userId = _tokenOpperation.GetUserIdFromToken();
            List<Note> notes = _noteService.GetNotesByUserId(userId);
            return Ok(notes);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }


    [HttpDelete("DeleteNote/{noteId}")]
    public IActionResult DeleteNote(int noteId)
    {

        try
        {
            int userId = _tokenOpperation.GetUserIdFromToken();
            _noteService.DeleteNoteById(userId, noteId);
            return Ok("Note Was Deleted");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }


    
}
