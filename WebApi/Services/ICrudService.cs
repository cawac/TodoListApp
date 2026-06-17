using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Services;

public interface ICrudService<TDto, TKey>
    where TDto : class
    where TKey : notnull
{
    Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TDto?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default);

    Task<TDto?> UpdateAsync(TKey id, TDto dto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
}
