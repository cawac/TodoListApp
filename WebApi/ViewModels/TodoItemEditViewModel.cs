using System.ComponentModel.DataAnnotations;
using WebApi.ViewModels.Validation;

namespace WebApi.ViewModels;

public class TodoItemEditViewModel : IValidatableObject
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Enter a title")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "Title: 3 to 300 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Notes: up to 2000 characters")]
    public string? Notes { get; set; }

    [Display(Name = "Due date")]
    [NotPastDate]
    public DateTimeOffset? DueAt { get; set; }

    [Display(Name = "Completed")]
    public bool IsCompleted { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrWhiteSpace(Notes) && Notes.Trim() == Title.Trim())
        {
            yield return new ValidationResult("Notes must not repeat the title");
        }
    }
}
