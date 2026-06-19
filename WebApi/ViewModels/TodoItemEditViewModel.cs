using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels;

public class TodoItemEditViewModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(300)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Notes { get; set; }

    [WebApi.ViewModels.Validation.NotPastDate]
    public DateTimeOffset? DueAt { get; set; }

    public bool IsCompleted { get; set; }
}
