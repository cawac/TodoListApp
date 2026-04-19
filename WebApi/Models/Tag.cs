using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class Tag
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public ICollection<TodoItemTag> ItemTags { get; set; } = new List<TodoItemTag>();
}