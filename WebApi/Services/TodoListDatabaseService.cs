using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DataClasses;
using Entity = WebApi.Models.TodoList;
using ItemEntity = WebApi.Models.TodoItem;

namespace WebApi.Services;

public class TodoListDatabaseService : CrudService<Entity, TodoListData, int>, ITodoListDatabaseService
{
    public TodoListDatabaseService(TodoListDbContext db) : base(db)
    {
    }

    protected override Entity ToEntity(TodoListData dto)
    {
        var entity = new Entity
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt
        };

        if (dto.Items != null && dto.Items.Count > 0)
        {
            entity.Items = dto.Items.Select(i => new ItemEntity
            {
                Id = i.Id,
                Title = i.Title,
                Notes = i.Notes,
                IsCompleted = i.IsCompleted,
                CreatedAt = i.CreatedAt,
                DueAt = i.DueAt,
                CompletedAt = i.CompletedAt,
                Priority = (WebApi.Models.TodoPriority)i.Priority
            }).ToList();
        }

        return entity;
    }

    protected override TodoListData ToDto(Entity entity)
    {
        var dto = new TodoListData
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            Items = entity.Items?.Select(i => new TodoItemData
            {
                Id = i.Id,
                TodoListId = i.TodoListId,
                Title = i.Title,
                Notes = i.Notes,
                IsCompleted = i.IsCompleted,
                CreatedAt = i.CreatedAt,
                DueAt = i.DueAt,
                CompletedAt = i.CompletedAt,
                Priority = (int)i.Priority
            }).ToList() ?? new List<TodoItemData>()
        };

        return dto;
    }

    protected override void UpdateEntity(Entity entity, TodoListData dto)
    {
        entity.Title = dto.Title;
        entity.Description = dto.Description;
    }
}
