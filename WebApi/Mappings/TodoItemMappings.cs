using WebApi.Dtos;
using WebApi.ViewModels;

namespace WebApi.Mappings;

public static class TodoItemMappings
{
    public static TodoItemSummaryViewModel ToSummaryViewModel(this TodoItemDto dto)
    {
        return new TodoItemSummaryViewModel
        {
            Id = dto.Id,
            TodoListId = dto.TodoListId,
            TodoListTitle = dto.TodoListTitle,
            Title = dto.Title,
            IsCompleted = dto.IsCompleted,
            DueAt = dto.DueAt,
            Priority = dto.Priority.ToString(),
        };
    }

    public static TodoItemDetailsViewModel ToDetailsViewModel(this TodoItemDto dto)
    {
        return new TodoItemDetailsViewModel
        {
            Id = dto.Id,
            TodoListId = dto.TodoListId,
            TodoListTitle = dto.TodoListTitle,
            Title = dto.Title,
            Notes = dto.Notes,
            Priority = dto.Priority.ToString(),
            IsCompleted = dto.IsCompleted,
            CreatedAt = dto.CreatedAt,
            DueAt = dto.DueAt,
            CompletedAt = dto.CompletedAt,
            Tags = dto.Tags,
        };
    }

    public static TodoItemEditViewModel ToEditViewModel(this TodoItemDto dto)
    {
        return new TodoItemEditViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Notes = dto.Notes,
            DueAt = dto.DueAt,
            IsCompleted = dto.IsCompleted,
        };
    }

    public static TodoItemDto ToDto(this TodoItemCreateViewModel vm)
    {
        return new TodoItemDto
        {
            TodoListId = vm.TodoListId,
            Title = vm.Title,
            Notes = vm.Notes,
            DueAt = vm.DueAt,
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }

    // Copies only the fields the edit form owns, so Priority, CreatedAt and tags stay as they were
    public static void ApplyTo(this TodoItemEditViewModel vm, TodoItemDto dto)
    {
        dto.Title = vm.Title;
        dto.Notes = vm.Notes;
        dto.DueAt = vm.DueAt;
        dto.IsCompleted = vm.IsCompleted;
    }
}
