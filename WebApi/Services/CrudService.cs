using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services;


public abstract class CrudService<TEntity, TDto, TKey> : ICrudService<TDto, TKey>
    where TEntity : class
    where TDto : class
    where TKey : notnull
{
    protected readonly DbContext _db;

    protected CrudService(DbContext db)
    {
        _db = db;
    }

    protected virtual IQueryable<TEntity> Query()
    {
        return _db.Set<TEntity>().AsQueryable();
    }

    public virtual async Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Query().ToListAsync(cancellationToken);
        return entities.Select(ToDto).ToList();
    }

    public virtual async Task<TDto?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
        if (entity is null)
        {
            entity = await Query().FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id), cancellationToken);
        }

        return entity is null ? null : ToDto(entity);
    }

    public virtual async Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = ToEntity(dto);
        _db.Set<TEntity>().Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public virtual async Task<TDto?> UpdateAsync(TKey id, TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
        if (entity is null)
        {
            entity = await Query().FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id), cancellationToken);
            if (entity is null)
                return null;
        }

        UpdateEntity(entity, dto);
        await _db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public virtual async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
        if (entity is null)
        {
            entity = await Query().FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id), cancellationToken);
            if (entity is null)
                return false;
        }

        _db.Set<TEntity>().Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    protected abstract TEntity ToEntity(TDto dto);

    protected abstract TDto ToDto(TEntity entity);

    protected abstract void UpdateEntity(TEntity entity, TDto dto);
}
