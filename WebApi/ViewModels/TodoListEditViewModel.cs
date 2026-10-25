using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.ViewModels;

public class TodoListEditViewModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Enter a title")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Title: 3 to 50 characters")]
    [Remote(action: "CheckTitle", controller: "TodoList", AdditionalFields = nameof(Id), ErrorMessage = "A todo list with this title already exists")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description: up to 1000 characters")]
    public string? Description { get; set; }
}
