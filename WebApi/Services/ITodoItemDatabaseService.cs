using WebApi.Dtos;

namespace WebApi.Services;

public interface ITodoItemDatabaseService : ICrudService<TodoItemDto, int>
{
}
