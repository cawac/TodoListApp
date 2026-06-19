using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels;

public class TodoListEditViewModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }
}
