using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Services;

public class TodoListDatabaseService : CrudService<TodoList, TodoListDto, int>, ITodoListDatabaseService
{
    public TodoListDatabaseService(TodoListDbContext db)
        : base(db)
    {
    }

    protected override IQueryable<TodoList> Query()
    {
        return Db.Set<TodoList>()
            .AsNoTracking()
            .Include(x => x.Items);
    }

    protected override TodoList ToEntity(TodoListDto dto)
    {
        return new TodoList
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt,
        };
    }

    protected override TodoListDto ToDto(TodoList entity)
    {
        return new TodoListDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            Items = entity.Items
                .Select(i => new TodoItemDto
                {
                    Id = i.Id,
                    TodoListId = i.TodoListId,
                    TodoListTitle = entity.Title,
                    Title = i.Title,
                    Notes = i.Notes,
                    IsCompleted = i.IsCompleted,
                    CreatedAt = i.CreatedAt,
                    DueAt = i.DueAt,
                    CompletedAt = i.CompletedAt,
                    Priority = i.Priority,
                })
                .ToList(),
        };
    }

    protected override void UpdateEntity(TodoList entity, TodoListDto dto)
    {
        entity.Title = dto.Title;
        entity.Description = dto.Description;
    }
}
