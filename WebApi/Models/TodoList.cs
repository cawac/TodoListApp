using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

    public class TodoList
    {
        public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<TodoItem> Items { get; set; } = new List<TodoItem>();
}
