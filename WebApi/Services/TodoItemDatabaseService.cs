using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.ViewModels;

namespace WebApi.Services;

public class TodoItemDatabaseService : CrudService<WebApi.Models.TodoItem, TodoItemData, int>, ITodoItemDatabaseService
{
    public TodoItemDatabaseService(TodoListDbContext db) : base(db)
    {
    }

    protected override WebApi.Models.TodoItem ToEntity(TodoItemData dto)
    {
        return new WebApi.Models.TodoItem
        {
            Id = dto.Id,
            TodoListId = dto.TodoListId,
            Title = dto.Title,
            Notes = dto.Notes,
            IsCompleted = dto.IsCompleted,
            CreatedAt = dto.CreatedAt,
            DueAt = dto.DueAt,
            CompletedAt = dto.CompletedAt,
            Priority = (WebApi.Models.TodoPriority)dto.Priority
        };
    }

    protected override TodoItemData ToDto(WebApi.Models.TodoItem entity)
    {
        var dto = new TodoItemData
        {
            Id = entity.Id,
            TodoListId = entity.TodoListId,
            Title = entity.Title,
            Notes = entity.Notes,
            IsCompleted = entity.IsCompleted,
            CreatedAt = entity.CreatedAt,
            DueAt = entity.DueAt,
            CompletedAt = entity.CompletedAt,
            Priority = (int)entity.Priority
        };

        if (entity.TodoList != null)
        {
            dto.TodoListTitle = entity.TodoList.Title;
        }
        else
        {
            var list = _db.Set<WebApi.Models.TodoList>().Find(entity.TodoListId);
            if (list != null) dto.TodoListTitle = list.Title;
        }

        var tags = _db.Set<WebApi.Models.TodoItemTag>()
            .Where(t => t.TodoItemId == entity.Id)
            .Include(t => t.Tag)
            .Select(t => t.Tag.Name)
            .ToList();

        dto.Tags = tags;

        return dto;
    }

    protected override void UpdateEntity(WebApi.Models.TodoItem entity, TodoItemData dto)
    {
        entity.Title = dto.Title;
        entity.Notes = dto.Notes;
        entity.DueAt = dto.DueAt;
        // Ensure CompletedAt is set when item becomes completed, and cleared when reactivated
        if (dto.IsCompleted)
        {
            if (entity.CompletedAt == null)
            {
                entity.CompletedAt = DateTimeOffset.UtcNow;
            }
        }
        else
        {
            entity.CompletedAt = null;
        }

        entity.IsCompleted = dto.IsCompleted;
        entity.Priority = (WebApi.Models.TodoPriority)dto.Priority;
    }
}
