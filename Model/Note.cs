using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apief;
public class Note
{

    public int Id { get; set; }
    public int NoteId { get; set; }
    public int CategoryId { get; set; }
    public string Description { get; set; } = "";
    public string Title { get; set; } = "";
    public bool Done { get; set; }


}