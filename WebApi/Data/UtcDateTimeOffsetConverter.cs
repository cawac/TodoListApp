using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebApi.Data;

public class UtcDateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public UtcDateTimeOffsetConverter()
        : base(v => v.ToUniversalTime(), v => v)
    {
    }
}
