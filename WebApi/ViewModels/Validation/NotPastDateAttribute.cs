using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApi.ViewModels.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class NotPastDateAttribute : ValidationAttribute, IClientModelValidator
{
    public NotPastDateAttribute()
    {
        ErrorMessage = "Due date cannot be in the past.";
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is DateTimeOffset dto)
        {
            return dto.Date >= DateTimeOffset.UtcNow.Date;
        }

        if (value is DateTime dt)
        {
            return dt.Date >= DateTime.UtcNow.Date;
        }

        return false;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.TryAdd("data-val", "true");
        context.Attributes.TryAdd("data-val-notpastdate", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
    }
}
