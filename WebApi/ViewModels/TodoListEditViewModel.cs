using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    // View model used when editing an existing todo list
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
}
