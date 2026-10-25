namespace WebApi.ViewModels;

public class TodoItemDetailsViewModel
{
    public int Id { get; set; }

    public int TodoListId { get; set; }

    public string? TodoListTitle { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public string Priority { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? DueAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public IReadOnlyList<string> Tags { get; set; } = [];
}
