using WebApi.Models;

namespace WebApi.Dtos;

public class TodoItemDto
{
    public int Id { get; set; }

    public int TodoListId { get; set; }

    public string? TodoListTitle { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public bool IsCompleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? DueAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public TodoPriority Priority { get; set; } = TodoPriority.Normal;

    public List<string> Tags { get; set; } = new();
}
