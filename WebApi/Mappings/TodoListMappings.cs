using WebApi.Dtos;
using WebApi.ViewModels;

namespace WebApi.Mappings;

public static class TodoListMappings
{
    public static TodoListGetViewModel ToGetViewModel(this TodoListDto dto)
    {
        return new TodoListGetViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt,
            Items = dto.Items.Select(i => i.ToSummaryViewModel()).ToList(),
        };
    }

    public static TodoListEditViewModel ToEditViewModel(this TodoListDto dto)
    {
        return new TodoListEditViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
        };
    }

    public static TodoListDto ToDto(this TodoListCreateViewModel vm)
    {
        return new TodoListDto
        {
            Title = vm.Title.Trim(),
            Description = vm.Description,
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }

    public static TodoListDto ToDto(this TodoListEditViewModel vm)
    {
        return new TodoListDto
        {
            Id = vm.Id,
            Title = vm.Title.Trim(),
            Description = vm.Description,
        };
    }
}
