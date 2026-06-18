using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotPastDateAttribute : ValidationAttribute
    {
        public NotPastDateAttribute()
        {
            ErrorMessage = "Due date cannot be in the past.";
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true; // not our job to require value

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
    }
}
