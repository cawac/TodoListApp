using System;

namespace WebApi.ViewModels
{
    // Lightweight summary for items inside a todo list used in list/details views
    public class TodoItemSummaryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        public string? Priority { get; set; }
    }
}
