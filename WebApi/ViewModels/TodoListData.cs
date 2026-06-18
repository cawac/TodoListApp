using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    public class TodoListData
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public List<TodoItemData> Items { get; set; } = new List<TodoItemData>();
    }

    public class TodoItemData
    {
        public int Id { get; set; }

        public int TodoListId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public bool IsCompleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? DueAt { get; set; }

        public DateTimeOffset? CompletedAt { get; set; }


        public int Priority { get; set; } = 1;
        public string? TodoListTitle { get; set; }
        public List<string>? Tags { get; set; }
    }
}
