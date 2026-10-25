namespace WebApi.Dtos;

public class TodoListDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public List<TodoItemDto> Items { get; set; } = new();
}
