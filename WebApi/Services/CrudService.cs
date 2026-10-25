using Microsoft.EntityFrameworkCore;

namespace WebApi.Services;

public abstract class CrudService<TEntity, TDto, TKey> : ICrudService<TDto, TKey>
    where TEntity : class
    where TDto : class
    where TKey : notnull
{
    protected CrudService(DbContext db)
    {
        Db = db;
    }

    protected DbContext Db { get; }

    public virtual async Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Query().ToListAsync(cancellationToken);
        return entities.Select(ToDto).ToList();
    }

    public virtual async Task<TDto?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await Query().FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id").Equals(id), cancellationToken);
        return entity is null ? null : ToDto(entity);
    }

    public virtual async Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = ToEntity(dto);
        Db.Set<TEntity>().Add(entity);
        await Db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public virtual async Task<TDto?> UpdateAsync(TKey id, TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await Db.Set<TEntity>().FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return null;
        }

        UpdateEntity(entity, dto);
        await Db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public virtual async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await Db.Set<TEntity>().FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return false;
        }

        Db.Set<TEntity>().Remove(entity);
        await Db.SaveChangesAsync(cancellationToken);
        return true;
    }

    protected virtual IQueryable<TEntity> Query()
    {
        return Db.Set<TEntity>();
    }

    protected abstract TEntity ToEntity(TDto dto);

    protected abstract TDto ToDto(TEntity entity);

    protected abstract void UpdateEntity(TEntity entity, TDto dto);
}
