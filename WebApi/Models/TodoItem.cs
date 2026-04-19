using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class TodoItem
{
    public int Id { get; set; }

    public int TodoListId { get; set; }

    public TodoList? TodoList { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public bool IsCompleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? DueAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public TodoPriority Priority { get; set; } = TodoPriority.Normal;

    public ICollection<TodoItemTag> ItemTags { get; set; } = new List<TodoItemTag>();
}