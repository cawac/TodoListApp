using System;
using System.Collections.Generic;

namespace WebApi.ViewModels;

public class TodoListGetViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public List<TodoItemSummaryViewModel> Items { get; set; } = new();
}
