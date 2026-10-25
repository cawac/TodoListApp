using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Services;

public class TodoItemDatabaseService : CrudService<TodoItem, TodoItemDto, int>, ITodoItemDatabaseService
{
    public TodoItemDatabaseService(TodoListDbContext db)
        : base(db)
    {
    }

    protected override IQueryable<TodoItem> Query()
    {
        return Db.Set<TodoItem>()
            .AsNoTracking()
            .Include(x => x.TodoList)
            .Include(x => x.ItemTags)
                .ThenInclude(t => t.Tag);
    }

    protected override TodoItem ToEntity(TodoItemDto dto)
    {
        return new TodoItem
        {
            Id = dto.Id,
            TodoListId = dto.TodoListId,
            Title = dto.Title,
            Notes = dto.Notes,
            IsCompleted = dto.IsCompleted,
            CreatedAt = dto.CreatedAt,
            DueAt = dto.DueAt,
            CompletedAt = dto.CompletedAt,
            Priority = dto.Priority,
        };
    }

    protected override TodoItemDto ToDto(TodoItem entity)
    {
        return new TodoItemDto
        {
            Id = entity.Id,
            TodoListId = entity.TodoListId,
            TodoListTitle = entity.TodoList?.Title,
            Title = entity.Title,
            Notes = entity.Notes,
            IsCompleted = entity.IsCompleted,
            CreatedAt = entity.CreatedAt,
            DueAt = entity.DueAt,
            CompletedAt = entity.CompletedAt,
            Priority = entity.Priority,
            Tags = entity.ItemTags
                .Where(t => t.Tag is not null)
                .Select(t => t.Tag!.Name)
                .ToList(),
        };
    }

    protected override void UpdateEntity(TodoItem entity, TodoItemDto dto)
    {
        entity.Title = dto.Title;
        entity.Notes = dto.Notes;
        entity.DueAt = dto.DueAt;
        entity.Priority = dto.Priority;

        if (!dto.IsCompleted)
        {
            entity.CompletedAt = null;
        }
        else if (entity.CompletedAt is null)
        {
            entity.CompletedAt = DateTimeOffset.UtcNow;
        }

        entity.IsCompleted = dto.IsCompleted;
    }
}
