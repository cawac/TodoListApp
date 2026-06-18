using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    // View model used when creating a new todo list
    public class TodoListCreateViewModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }
    }
}
