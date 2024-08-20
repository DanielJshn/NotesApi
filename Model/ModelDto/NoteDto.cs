using System.ComponentModel.DataAnnotations;

namespace apief;
public class NoteDto
{
    
    public int CategoryId { get; set; }
    public string Description { get; set; } = "";
    public string Title { get; set; } = "";
    public bool Done { get; set; }

}