using System;

namespace WebApi.ViewModels;

public class TodoItemSummaryViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTimeOffset? DueAt { get; set; }
    public string? Priority { get; set; }
}
