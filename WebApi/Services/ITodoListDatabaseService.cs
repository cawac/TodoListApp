using WebApi.Dtos;

namespace WebApi.Services;

public interface ITodoListDatabaseService : ICrudService<TodoListDto, int>
{
}
