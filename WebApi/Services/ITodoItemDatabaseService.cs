using WebApi.ViewModels;

namespace WebApi.Services;

public interface ITodoItemDatabaseService : ICrudService<TodoItemData, int>
{
}
