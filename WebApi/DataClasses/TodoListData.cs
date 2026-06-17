using System;
using System.Collections.Generic;

namespace WebApi.DataClasses
{
    public class TodoListData
    {
        public int Id { get; set; }

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

        /// <summary>
        /// 0 = Low, 1 = Normal, 2 = High, 3 = Urgent
        /// </summary>
        public int Priority { get; set; } = 1;
    }
}
