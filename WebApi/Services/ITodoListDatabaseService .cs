using WebApi.ViewModels;

namespace WebApi.Services;

public interface ITodoListDatabaseService : ICrudService<TodoListData, int>
{
}
