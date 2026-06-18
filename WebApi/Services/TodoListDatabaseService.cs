using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.ViewModels;
using Entity = WebApi.Models.TodoList;
using ItemEntity = WebApi.Models.TodoItem;

namespace WebApi.Services;

public class TodoListDatabaseService : CrudService<Entity, TodoListData, int>, ITodoListDatabaseService
{
    private readonly Microsoft.Extensions.Logging.ILogger<TodoListDatabaseService>? _logger;

    public TodoListDatabaseService(TodoListDbContext db, Microsoft.Extensions.Logging.ILogger<TodoListDatabaseService>? logger = null) : base(db)
    {
        _logger = logger;
    }

    protected override IQueryable<Entity> Query()
    {
        return _db.Set<Entity>()
            .AsQueryable()
            .Include(x => x.Items)
                .ThenInclude(i => i.ItemTags)
                    .ThenInclude(t => t.Tag);
    }

    public override async Task<TodoListData?> GetByIdAsync(int id, System.Threading.CancellationToken cancellationToken = default)
    {
        var entity = await Query().FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id), cancellationToken);
        if (entity == null) return null;
        return ToDto(entity);
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

        // Diagnostic logging to verify items mapping
        try
        {
            var count = dto.Items?.Count ?? 0;
            _logger?.LogDebug("ToDto: TodoList {Id} mapped with {Count} items", entity.Id, count);
            if (count > 0)
            {
                var titles = string.Join(", ", dto.Items!.Select(x => x.Title));
                _logger?.LogDebug("ToDto: Titles: {Titles}", titles);
            }
        }
        catch { }

        return dto;
    }

    protected override void UpdateEntity(Entity entity, TodoListData dto)
    {
        entity.Title = dto.Title;
        entity.Description = dto.Description;
    }
}
