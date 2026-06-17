using WebApi.DataClasses;

namespace WebApi.Services;

public interface ITodoListDatabaseService: ICrudService<TodoListData, int>
{
}
