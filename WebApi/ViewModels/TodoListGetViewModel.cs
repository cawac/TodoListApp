using System;
using System.Collections.Generic;

namespace WebApi.ViewModels
{
    // View model used to display a todo list and its items (GET)
    public class TodoListGetViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public List<TodoItemSummaryViewModel> Items { get; set; } = new();
    }
}
