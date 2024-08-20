using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace apief;

[Authorize]
[ApiController]
[Microsoft.AspNetCore.Mvc.Route("category[controller]")]
public class CategoryController : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly IMapper _mapper;
    private readonly TokenOpperation _tokenOpperation;
    public CategoryController(INoteService noteService, IMapper mapper, TokenOpperation tokenOpperation)
    {
        _noteService = noteService;
        _mapper = mapper;
        _tokenOpperation = tokenOpperation;
    }

    [HttpPost("PostCategory")]
    public IActionResult CreatedCategory(CategoryDto categoryDto)
    {
        try
        {
            int userId = _tokenOpperation?.GetUserIdFromToken() ?? throw new Exception("Token operation is null");
            CategoryDto mappedNoteDto = _mapper.Map<CategoryDto>(categoryDto);
            _noteService.AddCategory(userId, mappedNoteDto);

            return Ok(categoryDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteCategory/{categoryId}")]
    public IActionResult DeleteCategory(int categoryId)
    {
        try
        {
            int userId = _tokenOpperation?.GetUserIdFromToken() ?? throw new Exception("Token operation is null");
            _noteService.DeleteCategoryById(categoryId, userId);
            return Ok("Category succes deleted");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
